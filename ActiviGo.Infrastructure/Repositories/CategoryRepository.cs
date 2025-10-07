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
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithActivitiesAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.Activities.Any(a => a.Id == id))  
                .Include(c => c.Activities)
                .AsNoTracking()
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

        public async Task<Category?> FindByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
