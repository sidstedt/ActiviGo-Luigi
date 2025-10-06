using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ActiviGo.Infrastructure.Data;

namespace ActiviGo.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ActiviGoDbContext _dbContext;

        public BookingRepository(ActiviGoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            var booking = await _dbContext.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId, ct);
            if (booking == null)
                return false;

            booking.Status = BookingStatus.Canceled;
            booking.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<ActivityOccurrence?> GetActivityOccurrenceByIdAsync(int activityOccurrenceId, CancellationToken ct)
        {
            return await _dbContext.ActivityOccurrences
                .Include(a => a.Activity)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Zone)
                .Include(a => a.Bookings)
                .FirstOrDefaultAsync(a => a.Id == activityOccurrenceId, ct);
        }

        public async Task<Booking?> GetBookingForOccurrenceAsync(Guid userId, int occurrenceId, CancellationToken ct)
        {
            return await _dbContext.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ActivityOccurrenceId == occurrenceId && b.Status != BookingStatus.Canceled, ct);
        }

        public async Task<Booking> CreateBookingAsync(Guid userId, Booking booking, CancellationToken ct)
        {
            _dbContext.Add(booking);
            await _dbContext.SaveChangesAsync(ct);
            await _dbContext.Entry(booking).Reference(b => b.ActivityOccurrence).LoadAsync(ct);
            await _dbContext.Entry(booking.ActivityOccurrence).Reference(o => o.Activity).LoadAsync(ct);
            await _dbContext.Entry(booking.ActivityOccurrence.Activity).Reference(a => a.Category).LoadAsync(ct);
            await _dbContext.Entry(booking.ActivityOccurrence).Reference(o => o.Zone).LoadAsync(ct);
            return booking;
        }

        public async Task<List<Booking>> GetAllBookingsAsync(Guid userId, CancellationToken ct)
        {
            return await _dbContext.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Activity)
                        .ThenInclude(a => a.Category)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Zone)
                .ToListAsync(ct);
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            return await _dbContext.Bookings
                .Where(b => b.UserId == userId && b.Id == bookingId)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Activity)
                        .ThenInclude(a => a.Category)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Zone)
                .FirstOrDefaultAsync(ct);
        }
    }
}
