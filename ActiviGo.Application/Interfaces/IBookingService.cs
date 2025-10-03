using ActiviGo.Application.DTOs;

namespace ActiviGo.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync(Guid userId, CancellationToken ct);
        Task<BookingDto?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct);
        Task<CreatedBookingDto> CreateBookingAsync(Guid userId, CreateBookingDto dto, CancellationToken ct);
        Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct);
        Task<BookingDto?> UpdateBookingAsync(Guid userId, int bookingId, UpdateBookingDto dto, CancellationToken ct);
    }
}
