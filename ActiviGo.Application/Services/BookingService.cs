using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepo, IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _mapper = mapper;
        }
        public async Task<CreatedBookingDto> CreateBookingAsync(Guid userId, CreateBookingDto dto, CancellationToken ct)
        {
            var activityOccurence = await _bookingRepo.GetActivityOccurenceByIdAsync(dto.ActivityOccurenceId, ct);
            if (activityOccurence == null)
                throw new ArgumentException("ActivityOccurence not found");

            var currentCount = activityOccurence.Bookings.Count(b => b.Status != BookingStatus.Canceled);
            if (currentCount >= activityOccurence.Activity.MaxParticipants)
                throw new InvalidOperationException("ActivityOccurence is full.");

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

            // Mapper används för att skapa CreatedBookingDto
            return _mapper.Map<CreatedBookingDto>(created);
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync(Guid userId, CancellationToken ct)
        {
            var allBookings = await _bookingRepo.GetAllBookingsAsync(userId, ct);
            return _mapper.Map<List<BookingDto>>(allBookings);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            var b = await _bookingRepo.GetBookingByIdAsync(userId, bookingId, ct);
            return b == null ? null : _mapper.Map<BookingDto>(b);
        }

        public async Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            return await _bookingRepo.CancelBookingAsync(userId, bookingId, ct);
        }

        public async Task<BookingDto?> UpdateBookingAsync(Guid userId, int bookingId, UpdateBookingDto dto, CancellationToken ct)
        {
            // Hämta befintlig bokning
            var existing = await _bookingRepo.GetBookingByIdAsync(userId, bookingId, ct);
            if (existing == null) return null;

            // Uppdatera fält via AutoMapper
            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            // Spara ändringar
            await _bookingRepo.UpdateAsync(existing);

            // Hämta bokning igen för att inkludera relaterade entiteter
            var updated = await _bookingRepo.GetBookingByIdAsync(userId, bookingId, ct);
            return updated == null ? null : _mapper.Map<BookingDto>(updated);
        }
    }
}
