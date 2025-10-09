using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ActiviGo.Application.Services
{
    public class CategoryService
        : GenericService<Category, CategoryDto, CreateCategoryDto, CategoryUpdateDto>, ICategoryService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitofWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
            : base(unitOfWork.Category, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddActivityToCategory(int categoryId, int activityId)
        {
            await _unitOfWork.Category.AddActivityToCategoryAsync(categoryId, activityId);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                $"Added activity with ID {activityId} to category with ID {categoryId}");
        }

        public async Task<IEnumerable<CreateCategoryDto>> GetCategories()
        {
            var categories = await _unitOfWork.Category.GetAllCategoriesAsync();

            return _mapper.Map<IEnumerable<CreateCategoryDto>>(categories);
        }

        public async Task<IEnumerable<CategoryWithActivitiesDto>> GetAllCategoriesWithActivities()
        {
            var categories = await _unitOfWork.Category.GetAllCategoriesWithActivitiesAsync();

            if (!categories.Any())
                _logger.LogWarning("No categories with activities found.");

            return _mapper.Map<IEnumerable<CategoryWithActivitiesDto>>(categories);
        }

        public async Task<CategoryWithActivitiesDto?> GetCategoryWithActivitiesById(int categoryId)
        {
            var category = await _unitOfWork.Category.GetCategoryWithActivitiesByIdAsync(categoryId);

            if (category == null)
            {
                _logger.LogWarning($"Category with ID {categoryId} not found.");
                return null;
            }

            return _mapper.Map<CategoryWithActivitiesDto>(category);
        }

        public async Task RemoveActivityFromCategory(int categoryId, int activityId)
        {
            await _unitOfWork.Category.RemoveActivityFromCategoryAsync(categoryId, activityId);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                $"Removed activity with ID {activityId} from category with ID {categoryId}");
        }
    }
}
