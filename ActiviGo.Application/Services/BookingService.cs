using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Application.Services;
using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

public class BookingService : IBookingService
{
    private readonly IUnitofWork _uow;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<BookingService> _logger;

    public BookingService(
        IUnitofWork uow,
        IMapper mapper,
        IEmailService emailService,
        UserManager<User> userManager,
        ILogger<BookingService> logger)
    {
        _uow = uow;
        _mapper = mapper;
        _emailService = emailService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<CreatedBookingDto> CreateBookingAsync(Guid userId, CreateBookingDto dto, CancellationToken ct)
    {
        _logger.LogDebug("Start CreateBooking: UserId={UserId} ActivityOccurrenceId={OccurrenceId}", userId, dto.ActivityOccurrenceId);

        var activityOccurrence = await _uow.ActivityOccurrence.GetActivityOccurrenceByIdAsync(dto.ActivityOccurrenceId, ct);
        if (activityOccurrence == null)
        {
            _logger.LogWarning("ActivityOccurrence saknas: OccurrenceId={OccurrenceId}", dto.ActivityOccurrenceId);
            throw new KeyNotFoundException("ActivityOccurrence not found");
        }

        var currentCount = await _uow.Booking.GetActiveBookingCountAsync(dto.ActivityOccurrenceId, ct);
        if (currentCount >= activityOccurrence.Activity.MaxParticipants)
        {
            _logger.LogWarning("Fullt: OccurrenceId={OccurrenceId}", dto.ActivityOccurrenceId);
            throw new InvalidOperationException("ActivityOccurrence is full.");
        }

        var existing = await _uow.Booking.GetBookingForOccurrenceAsync(userId, dto.ActivityOccurrenceId, ct);
        if (existing != null)
        {
            _logger.LogWarning("Dubbelbokningsförsök: UserId={UserId} OccurrenceId={OccurrenceId}", userId, dto.ActivityOccurrenceId);
            throw new InvalidOperationException("Booking already exists for this occurrence.");
        }

        var booking = new Booking
        {
            UserId = userId,
            ActivityOccurrenceId = dto.ActivityOccurrenceId,
            Status = BookingStatus.Reserved,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _uow.Booking.CreateBookingAsync(booking, ct);
        await _uow.SaveChangesAsync();

        _logger.LogInformation("Bokning skapad: BookingId={BookingId} UserId={UserId} OccurrenceId={OccurrenceId}", created.Id, userId, dto.ActivityOccurrenceId);

        // Skicka bekräftelsemail
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Kunde inte hitta användare med ID {UserId} för e-postbekräftelse.", userId);
            }
            else
            {
                var activity = activityOccurrence.Activity;

                var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "BookingConfirmationTemplate.html");
                var body = await File.ReadAllTextAsync(templatePath);

                // Ersätt placeholders med rätt värden
                body = body.Replace("{{FirstName}}", user.UserName ?? "Kund")
                           .Replace("{{ActivityName}}", activity.Name)
                           .Replace("{{Date}}", activityOccurrence.StartTime.ToString("yyyy-MM-dd"))
                           .Replace("{{StartTime}}", activityOccurrence.StartTime.ToString("HH:mm"))
                           .Replace("{{EndTime}}", activityOccurrence.EndTime.ToString("HH:mm"))
                           .Replace("{{Location}}", activityOccurrence.Zone?.Name ?? "Okänd plats")
                           .Replace("{{Status}}", booking.Status.ToString())
                           .Replace("{{Year}}", DateTime.Now.Year.ToString());

                await _emailService.SendEmailAsync(user.Email, "Bekräftelse på din bokning – ActiviGo", body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fel vid försök att skicka bokningsbekräftelse till användare med ID {UserId}.", userId);
        }


        return _mapper.Map<CreatedBookingDto>(created);
    }
    public async Task<List<BookingDto>> GetAllBookingsAsync(Guid userId, CancellationToken ct)
    {
        _logger.LogDebug("Hämtar alla bokningar: UserId={UserId}", userId);
        var allBookings = await _uow.Booking.GetAllBookingsAsync(userId, ct);
        return _mapper.Map<List<BookingDto>>(allBookings);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct)
    {
        _logger.LogDebug("Hämtar bokning: UserId={UserId} BookingId={BookingId}", userId, bookingId);
        var b = await _uow.Booking.GetBookingByIdAsync(userId, bookingId, ct);
        if (b == null)
        {
            _logger.LogDebug("Bokning hittades inte: UserId={UserId} BookingId={BookingId}", userId, bookingId);
            return null;
        }
        return _mapper.Map<BookingDto>(b);
    }

    public async Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct)
    {
        _logger.LogDebug("Avbokningsförsök: UserId={UserId} BookingId={BookingId}", userId, bookingId);
        var success = await _uow.Booking.CancelBookingAsync(userId, bookingId, ct);
        if (!success)
        {
            _logger.LogWarning("Avbokning misslyckades (saknas eller ej ägare): UserId={UserId} BookingId={BookingId}", userId, bookingId);
            return false;
        }

        await _uow.SaveChangesAsync();
        _logger.LogInformation("Bokning avbokad: UserId={UserId} BookingId={BookingId}", userId, bookingId);
        return true;
    }

    // Staff scope
    public async Task<List<BookingDto>?> GetBookingsForOccurrenceAsync(int occurrenceId, Guid staffId, CancellationToken ct)
    {
        _logger.LogDebug("Staff bokningslista: StaffId={StaffId} OccurrenceId={OccurrenceId}", staffId, occurrenceId);

        var occurrence = await _uow.ActivityOccurrence.GetActivityOccurrenceByIdAsync(occurrenceId, ct);
        if (occurrence == null)
        {
            _logger.LogWarning("Occurrence saknas: OccurrenceId={OccurrenceId}", occurrenceId);
            return null;
        }

        if (occurrence.Activity.StaffId != staffId)
        {
            _logger.LogWarning("Åtkomst nekad för bookings: StaffId={StaffId} OccurrenceId={OccurrenceId}", staffId, occurrenceId);
            return null;
        }

        var bookings = occurrence.Bookings != null
            ? occurrence.Bookings.ToList()
            : await _uow.Booking.GetBookingsForOccurrenceAsync(occurrenceId, ct);

        return _mapper.Map<List<BookingDto>>(bookings);
    }
}
