using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ActiviGo.Infrastructure.Data;

namespace ActiviGo.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ActiviGoDbContext _ctx;

        public BookingRepository(ActiviGoDbContext dbContext)
        {
            _ctx = dbContext;
        }

        public async Task<bool> CancelBookingAsync(Guid userId, int bookingId, CancellationToken ct)
        {
            var booking = await _ctx.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId, ct);
            if (booking == null)
                return false;

            booking.Status = BookingStatus.Canceled;
            booking.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<Booking?> GetBookingForOccurrenceAsync(Guid userId, int occurrenceId, CancellationToken ct)
        {
            return await _ctx.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ActivityOccurrenceId == occurrenceId && b.Status != BookingStatus.Canceled, ct);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken ct)
        {
            await _ctx.AddAsync(booking, ct);

            await _ctx.Entry(booking).Reference(b => b.ActivityOccurrence).LoadAsync(ct);
            await _ctx.Entry(booking.ActivityOccurrence).Reference(o => o.Activity).LoadAsync(ct);
            await _ctx.Entry(booking.ActivityOccurrence.Activity).Reference(a => a.Category).LoadAsync(ct);
            await _ctx.Entry(booking.ActivityOccurrence).Reference(o => o.Zone).LoadAsync(ct);
            return booking;
        }

        public async Task<Booking?> GetAnyBookingForOccurrenceAsync(Guid userId, int occurrenceId, CancellationToken ct)
        {
            return await _ctx.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ActivityOccurrenceId == occurrenceId, ct);
        }

        public async Task<List<Booking>> GetAllBookingsAsync(Guid userId, CancellationToken ct)
        {
            return await _ctx.Bookings
                .AsNoTracking()
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
            return await _ctx.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == userId && b.Id == bookingId)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Activity)
                        .ThenInclude(a => a.Category)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Zone)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<int> GetActiveBookingCountAsync(int occurrenceId, CancellationToken ct)
        {
            return await _ctx.Bookings
                .Where(b => b.ActivityOccurrenceId == occurrenceId && b.Status != BookingStatus.Canceled)
                .CountAsync(ct);
        }

        // Staff scope
        public async Task<List<Booking>> GetBookingsForOccurrenceAsync(int occurrenceId, CancellationToken ct)
        {
            return await _ctx.Bookings
                .Where(b => b.ActivityOccurrenceId == occurrenceId)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(o => o.Activity)
                        .ThenInclude(a => a.Category)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(o => o.Zone)
                .ToListAsync(ct);
        }

        public async Task<List<Booking>> GetAllBookingsAdminAsync(CancellationToken ct)
        {
            return await _ctx.Bookings
                .AsNoTracking()
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Activity)
                        .ThenInclude(a => a.Category)
                .Include(b => b.ActivityOccurrence)
                    .ThenInclude(ao => ao.Zone)
                .ToListAsync(ct);
        }
    }
}
