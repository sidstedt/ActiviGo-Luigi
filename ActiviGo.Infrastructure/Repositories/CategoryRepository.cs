using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ActiviGoDbContext _context;
        public CategoryRepository(ActiviGoDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var removeCategory = await _context.Categories.FindAsync(categoryId);

            if (removeCategory != null) { return false; }

            _context.Categories.Remove(removeCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Activities)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithActivitiesAsync(string activityName)
        {
            return await _context.Categories
             .AsNoTracking()
             .Include(c => c.Activities.Where(a => a.Name == activityName)) 
             .Where(c => c.Activities.Any(a => a.Name == activityName))
             .ToListAsync();
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var updateCategory = await _context.Categories.FindAsync(category.Id);

            if (updateCategory is null) return null;

            _context.Categories.Update(updateCategory);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
