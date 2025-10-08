using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface ICategoryService : IGenericService<Category, CategoryDto, CreateCategoryDto, CategoryUpdateDto>
    {
        Task<IEnumerable<CategoryDto>> GetCategoryWithActivitiesById(int categoryId);

        Task<IEnumerable<CategoryDto>> GetAllCategoriesWithActivities();
        Task AddActivityToCategory(int categoryId, int activityId);
        Task RemoveActivityFromCategory(int categoryId, int activityId);
    }

}
