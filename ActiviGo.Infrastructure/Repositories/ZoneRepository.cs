using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ZoneRepository : GenericRepository<Zone>,IZoneRepository
    {
        public ZoneRepository(ActiviGoDbContext contex) : base(contex) 
        {
        }
     
        public async Task<IEnumerable<Zone>> GetZoneWithActivitiesAsync(int id)
        {
            return await _dbSet
                .Include(z => z.Activities)
                .Where(z => z.Id == id)
                .ToListAsync();
        }
    }
}
