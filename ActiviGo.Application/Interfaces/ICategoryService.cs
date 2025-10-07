using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface ICategoryService : IGenericService<Category, CategoryDto, CreateCategoryDto, CategoryUpdateDto>
    {
        Task<Category?> GetCategoryWithActivitiesById(int categoryId, CancellationToken ct);
    }
}
