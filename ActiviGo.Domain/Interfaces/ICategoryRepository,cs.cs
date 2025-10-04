using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesWithActivitiesAsync(string activityName);
        Task<Category>CreateCategoryAsync(Category category);
        Task<Category>UpdateCategoryAsync(Category category);
        Task <bool>DeleteCategoryAsync(int categoryId);
    }
}
