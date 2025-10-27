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

        public override async Task<IEnumerable<Activity>> GetAllAsync()//Retrieves all activities, including their associated Category and Zone.
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .ToListAsync();
        }

        public override async Task<Activity?> GetByIdAsync(int id)//Retrieves a specific activity by its ID, including its associated Category and Zone.
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetByStaffAsync(Guid staffId, CancellationToken ct)//Retrieves a collection of activities associated with a specific staff member.
        {
            return await _context.Activities
                .Where(a => a.StaffId == staffId)
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .ToListAsync(ct);
        }
    }
}
