using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ActiviGo.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitofWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IUnitofWork uow, IMapper mapper, ILogger<BookingService> logger)
        {
            _uow = uow;
            _mapper = mapper;
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
                _logger.LogWarning("Fullt: OccurrenceId={OccurrenceId} Current={Current} Max={Max}", dto.ActivityOccurrenceId, currentCount, activityOccurrence.Activity.MaxParticipants);
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
    }
}
