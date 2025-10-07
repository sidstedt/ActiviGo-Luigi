using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ActiviGoDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetCategoriesWithActivitiesAsync(int id)
        {
            return await _dbSet.Include(c =>  c.Activities)
                .Where(c => c.Activities
                .Any(a => a.Id == id))  
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
