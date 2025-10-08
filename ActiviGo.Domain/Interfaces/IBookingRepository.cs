using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllBookingsAsync(Guid userId, CancellationToken ct);
        Task<Booking?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct);
        Task<Booking?> GetBookingForOccurrenceAsync(Guid userId, int occurrenceId, CancellationToken ct);
        Task<Booking> CreateBookingAsync(Booking booking, CancellationToken ct);
        Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct);
        Task<int> GetActiveBookingCountAsync(int occurrenceId, CancellationToken ct);

        // Staff scope
        Task<List<Booking>> GetBookingsForOccurrenceAsync(int occurrenceId, CancellationToken ct);
    }
}
