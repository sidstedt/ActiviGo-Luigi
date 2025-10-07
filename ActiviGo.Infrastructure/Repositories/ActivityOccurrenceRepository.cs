using ActiviGo.Domain.Interfaces;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ActiviGo.Domain.Models;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ActivityOccurrenceRepository
    : GenericRepository<ActivityOccurrence>, IActivityOccurrenceRepository
    {
        public ActivityOccurrenceRepository(ActiviGoDbContext context)
            : base(context)
        {
        }

        public async Task<bool> CheckZoneAvailabilityAsync(int zoneId, DateTime startTime, DateTime endTime)
        {
            bool collisionExists = await _context.ActivityOccurrences
                .Where(ao => ao.ZoneId == zoneId)
                .AnyAsync(ao =>
                    ao.StartTime < endTime &&
                    ao.EndTime > startTime);

            return !collisionExists;
        }

        public async Task<int> GetCurrentParticipantCountAsync(int occurrenceId)
        {
            return await _context.Bookings
                .Where(b => b.ActivityOccurrenceId == occurrenceId)
                .CountAsync();
        }

        public override async Task<IEnumerable<ActivityOccurrence>> GetAllAsync()
        {
            return await _context.ActivityOccurrences
                .Include(ao => ao.Activity)
                .Include(ao => ao.Zone)
                .Include(ao => ao.Bookings)
                .ToListAsync();
        }

        public override async Task<ActivityOccurrence?> GetByIdAsync(int id)
        {
            return await _context.ActivityOccurrences
                .Include(ao => ao.Activity)
                .Include(ao => ao.Zone)
                .Include(ao => ao.Bookings)
                .FirstOrDefaultAsync(ao => ao.Id == id);
        }

        public async Task<ICollection<ActivityOccurrence>> GetOccurrencesByActivityIdAsync(int activityId)
        {
            return await _context.ActivityOccurrences
                .Where(ao => ao.ActivityId == activityId)
                .Include(ao => ao.Activity)
                .Include(ao => ao.Zone)
                .Include(ao => ao.Bookings)
                .ToListAsync();
        }
        public async Task<ActivityOccurrence?> GetActivityOccurrenceByIdAsync(int activityOccurrenceId, CancellationToken ct)
        {
            return await _context.ActivityOccurrences
                .Include(a => a.Activity)
                    .ThenInclude(a => a.Category)
                .Include(a => a.Zone)
                .Include(a => a.Bookings)
                .FirstOrDefaultAsync(a => a.Id == activityOccurrenceId, ct);
        }
    }
}