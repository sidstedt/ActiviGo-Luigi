using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        public BookingService(IBookingRepository bookings)
        {
            _bookingRepo = bookings;
        }
        public async Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            return await _bookingRepo.CancelBookingAsync(userId, bookingId, ct);
        }

        public async Task<CreatedBookingDto> CreateBookingAsync(Guid userId, CreateBookingDto dto, CancellationToken ct)
        {
            // Hämta ActivityOccurence från repository för att validera att den finns
            var activityOccurence = await _bookingRepo.GetActivityOccurenceByIdAsync(dto.ActivityOccurenceId, ct);
            if (activityOccurence == null)
                throw new ArgumentException("ActivityOccurence not found");

            // Kontrollera om det finns plats kvar
            var currentCount = activityOccurence.Bookings.Count(b => b.Status != BookingStatus.Canceled);
            if (currentCount >= activityOccurence.Activity.MaxParticipants)
                throw new InvalidOperationException("ActivityOccurence is full.");

            // Kontroll för att förhindra dubbelbokning
            var existing = await _bookingRepo.GetBookingForOccurenceAsync(userId, dto.ActivityOccurenceId, ct);
            if (existing != null)
                throw new InvalidOperationException("Booking already exists for this occurence.");

            var booking = new Booking
            {
                UserId = userId,
                ActivityOccurenceId = dto.ActivityOccurenceId,
                Status = BookingStatus.Reserved,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _bookingRepo.CreateBookingAsync(userId, booking, ct);

            return new CreatedBookingDto
            {
                Id = created.Id,
                ActivityOccurenceId = created.ActivityOccurenceId,
                ActivityId = activityOccurence.ActivityId,
                StartTime = activityOccurence.StartTime,
                EndTime = activityOccurence.EndTime
            };
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync(Guid userId, CancellationToken ct)
        {
            var allBookings = await _bookingRepo.GetAllBookingsAsync(userId, ct);
            return allBookings.Select(b => new BookingDto 
            {
                Id = b.Id,
                UserId = b.UserId,
                ActivityOccurenceId = b.ActivityOccurenceId,
                ActivityId = b.ActivityOccurence.ActivityId,
                ActivityName = b.ActivityOccurence.Activity.Name,
                ActivityDescription = b.ActivityOccurence.Activity.Description,
                ActivityPrice = b.ActivityOccurence.Activity.Price,
                StartTime = b.ActivityOccurence.StartTime,
                EndTime = b.ActivityOccurence.EndTime,
                ZoneName = b.ActivityOccurence.Zone.Name,
                CategoryName = b.ActivityOccurence.Activity.Category.Name,
                Status = b.Status
            }).ToList();
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            var b = await _bookingRepo.GetBookingByIdAsync(userId, bookingId, ct);
            if (b == null)
                return null;

            return new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                ActivityOccurenceId = b.ActivityOccurenceId,
                ActivityId = b.ActivityOccurence.ActivityId,
                ActivityName = b.ActivityOccurence.Activity.Name,
                ActivityDescription = b.ActivityOccurence.Activity.Description,
                ActivityPrice = b.ActivityOccurence.Activity.Price,
                StartTime = b.ActivityOccurence.StartTime,
                EndTime = b.ActivityOccurence.EndTime,
                ZoneName = b.ActivityOccurence.Zone.Name,
                CategoryName = b.ActivityOccurence.Activity.Category.Name,
                Status = b.Status
            };
        }
    }
}
