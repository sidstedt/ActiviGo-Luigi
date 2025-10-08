using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ActiviGo.Infrastructure.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(ActiviGoDbContext context) : base(context) { }

        public async Task<IEnumerable<Location>> GetAllIncludingAsync(params Expression<Func<Location, object>>[] includes)
        {
            IQueryable<Location> query = _context.Locations;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<Location?> GetByIdIncludingAsync(int id, params Expression<Func<Location, object>>[] includes)
        {
            IQueryable<Location> query = _context.Locations;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(l => l.Id == id);
        }

    }
}
