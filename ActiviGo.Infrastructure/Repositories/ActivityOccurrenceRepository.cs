using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ActivityOccurrence = ActiviGo.Domain.Models.ActivityOccurrence;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ActivityOccurrenceRepository : IActivityOccurrenceRepository
    {
        private readonly ActiviGoDbContext _context;

        public ActivityOccurrenceRepository(ActiviGoDbContext context)
        {
            _context = context;
        }


        public async Task<ActivityOccurrence> AddOccurrenceAsync(ActivityOccurrence occurrence)
        {

            _context.ActivityOccurrences.Add(occurrence);
            await _context.SaveChangesAsync();
            return occurrence;
        }

        public async Task<ICollection<ActivityOccurrence>> GetAllOccurrencesAsync()
        {

            return await _context.ActivityOccurrences
                .Include(ao => ao.Activity)
                .Include(ao => ao.Zone)
                .Include(ao => ao.Bookings) 
                .ToListAsync();
        }


        public async Task<ActivityOccurrence?> GetOccurrenceByIdAsync(int id)
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


        public async Task<ActivityOccurrence> UpdateOccurrenceAsync(ActivityOccurrence occurrence)
        {
            _context.Entry(occurrence).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return occurrence;
        }

        public async Task<bool> DeleteOccurrenceAsync(int id)
        {
            var occurrenceToDelete = await _context.ActivityOccurrences.FindAsync(id);
            if (occurrenceToDelete == null) return false;

            _context.ActivityOccurrences.Remove(occurrenceToDelete);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
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

        public Task<int> GetCurrentParticipantCountAsync(int occurrenceId)
        {
            return _context.Bookings
                .Where(b => b.ActivityOccurrenceId == occurrenceId)
                .CountAsync();
        }
    }
}