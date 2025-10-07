using ActiviGo.Application.DTOs;
using ActiviGo.Application.DTOs.CategoryDtos;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface ICategoryService : IGenericService<Category, CategoryReadDto, CreateCategoryDto, CategoryUpdateDto>
    {
        Task<Category?> GetCategoryWithActivitiesById(int categoryId, CancellationToken ct);
    }
}
