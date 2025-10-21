using ActiviGo.Application.DTOs;

namespace ActiviGo.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync(Guid userId, CancellationToken ct);
        Task<BookingDto?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct);
        Task<CreatedBookingDto> CreateBookingAsync(Guid userId, CreateBookingDto dto, CancellationToken ct);
        Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct);

        // Staff scope
        Task<List<BookingDto>?> GetBookingsForOccurrenceAsync(int occurrenceId, Guid staffId, CancellationToken ct);
        Task<List<BookingDto>> GetAllBookingsAdminAsync(CancellationToken ct);
    }
}
