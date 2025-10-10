using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ActiviGoDbContext context) : base(context) { }

        public async Task AddActivityToCategoryAsync(int categoryId, int activityId)
        {
            var category = await _dbSet
                .Include(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            var activity = await _context.Activities.FindAsync(activityId);

            if (category != null && activity != null)
            {
                category.Activities.Add(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesWithActivitiesAsync()
        {
            return await _dbSet
                .Include(c => c.Activities)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithActivitiesByIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task RemoveActivityFromCategoryAsync(int categoryId, int activityId)
        {
            var category = await _dbSet
                .Include(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null) return;

            var activity = category.Activities.FirstOrDefault(a => a.Id == activityId);
            if (activity == null) return;

            _context.Activities.Remove(activity);

            await _context.SaveChangesAsync();
        }
    }
}
