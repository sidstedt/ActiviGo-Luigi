using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ZoneRepository : GenericRepository<Zone>, IZoneRepository
    {
        public ZoneRepository(ActiviGoDbContext contex) : base(contex)
        {
        }

        public async Task AddActivityToZoneAsync(int zoneId, int activityId)
        {
            var zone = await _dbSet
                 .Include(Z => Z.Activities)
                 .FirstOrDefaultAsync(z => z.Id == zoneId);

            var activity = await _context.Activities.FindAsync(activityId);

            if (zone != null && activity != null)
            {
                zone.Activities.Add(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Zone>> GetAllZonesWithActivitiesAndLocationAsync()
        {
            return await _dbSet
                .Include(z => z.Activities)
                .Include(z => z.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Zone>> GetZonesByLocationIdAsync(int locationId)
        {
            return await _dbSet
                .Include(z => z.Location)
                .Where(z => z.LocationId == locationId)
                .ToListAsync();

        }

        public async Task<IEnumerable<Zone>> GetZoneWithActivitiesAsync(int id)
        {
            return await _dbSet
                .Include(z => z.Activities)
                .Where(z => z.Id == id) 
                .ToListAsync();
        }

        public async Task RemoveActivityFromZoneAsync(int zoneId, int activityId)
        {
            var zone = await _dbSet
                .Include(z => z.Activities)
                .Where(z => z.Id == zoneId)
                .FirstOrDefaultAsync();

            if (zone == null) return;

            var activity = zone.Activities.FirstOrDefault(a => a.Id == activityId);
            if (activity == null) return;

            _context.Activities.Remove(activity);

            await _context.SaveChangesAsync();

        }
    }
}
