using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category?>>GetCategoryWithActivitiesByIdAsync(int categoryId);

        //fetch all categories with activities
        Task<IEnumerable<Category>> GetAllCategoriesWithActivitiesAsync();

        //add activity to a category
        Task AddActivityToCategoryAsync(int categoryId, int activityId);

        Task<IEnumerable<Category>> GetAllCategories();

        //remove activity from a category
        Task RemoveActivityFromCategoryAsync(int categoryId, int activityId);
    }
}
