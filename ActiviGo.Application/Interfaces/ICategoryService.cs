using ActiviGo.Application.DTOs;
using ActiviGo.Application.DTOs.CategoryDtos;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int categoryId, CancellationToken ct);
        Task<Category?> GetCategoryWithActivities(int categoryId, CancellationToken ct);
        Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken ct);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<CategoryReadDto> UpdateCategoryAsync(int id, CategoryUpdateDto updateCategoryDto, CancellationToken ct);
    }
}
