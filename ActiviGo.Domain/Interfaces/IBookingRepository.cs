using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<ActivityOccurence?> GetActivityOccurenceByIdAsync(int activityOccurenceId, CancellationToken ct);
        Task<List<Booking>> GetAllBookingsAsync(Guid userId, CancellationToken ct);
        Task<Booking?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct);
        Task<Booking?> GetBookingForOccurenceAsync(Guid userId, int occurenceId, CancellationToken ct); // ny
        Task<Booking> CreateBookingAsync(Guid userId, Booking booking, CancellationToken ct);
        Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct);
    }
}
