using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly ActiviGoDbContext _context;
        public ZoneRepository(ActiviGoDbContext contex) 
        {
            _context = contex;
        }
        public async Task<Zone> CreateZoneAsync(Zone zone)
        {
            await _context.Zones.AddAsync(zone);
            await _context.SaveChangesAsync();
            return zone;
        }

        public async Task<bool> DeleteZoneAsync(int zoneId)
        {
            var delete = await _context.Zones.FindAsync(zoneId);

            if (delete != null) return false;

            _context.Zones.Remove(delete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Zone>> GetAllZonesAsync()
        {
            return await _context.Zones.ToListAsync();
        }

        public async Task<IEnumerable<Zone>> GetZoneWithActivitiesAsync(int id)
        {
            return await _context.Zones
                .Include(z => z.Activities)
                .ToListAsync();
        }

        public async Task<Zone> UpdateZoneAsync(Zone zone)
        {
            var update = await _context.Zones.FindAsync(zone.Id);

            if(update == null) return null;

            _context.Zones.Update(zone);

            await _context.SaveChangesAsync();
            return zone;
        }

        public async Task<Zone?> FindByIdAsync(int id)
        {
            return await _context.Zones.FindAsync(id);
        }
    }
}
