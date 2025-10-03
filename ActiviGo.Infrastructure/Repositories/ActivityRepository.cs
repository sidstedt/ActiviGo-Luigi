using ActiviGo.Domain.Interfaces;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Activity = ActiviGo.Domain.Models.Activity;

namespace ActiviGo.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ActiviGoDbContext _context; 

        public ActivityRepository(ActiviGoDbContext context)
        {
            _context = context;
        }

        public async Task<Activity> AddActivityAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<ICollection<Activity>> GetAllActivitiesAsync()
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .ToListAsync();
        }

        public async Task<Activity?> GetActivityByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Zone)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Activity> UpdateActivityAsync(Activity activity)
        {
            _context.Entry(activity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            var activityToDelete = await _context.Activities.FindAsync(id);
            if (activityToDelete == null) return false;

            _context.Activities.Remove(activityToDelete);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}