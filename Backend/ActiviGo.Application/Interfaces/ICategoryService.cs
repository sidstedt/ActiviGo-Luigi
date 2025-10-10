using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface ICategoryService : IGenericService<Category, CategoryDto, CreateCategoryDto, CategoryUpdateDto>
    {
        Task<CategoryWithActivitiesDto> GetCategoryWithActivitiesById(int categoryId);
        Task<IEnumerable<CreateCategoryDto>> GetCategories();

        Task<IEnumerable<CategoryWithActivitiesDto>> GetAllCategoriesWithActivities();
        Task AddActivityToCategory(int categoryId, int activityId);
        Task RemoveActivityFromCategory(int categoryId, int activityId);
    }

}
