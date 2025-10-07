using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ActiviGoDbContext context) : base(context) { }

        public Task AddActivityToCategoryAsync(int categoryId, int activityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllCategoriesWithActivitiesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetCategoryWithActivitiesByIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Activities)
                .Where(c => c.Id == categoryId)
                .ToListAsync();
            //.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public Task RemoveActivityFromCategoryAsync(int categoryId, int activityId)
        {
            throw new NotImplementedException();
        }
    }
}
