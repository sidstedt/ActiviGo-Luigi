using ActiviGo.Domain.Models;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(ActiviGoDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .ToListAsync();
        }

        public override async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
