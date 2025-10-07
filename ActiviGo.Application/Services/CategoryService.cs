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
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoryWithActivitiesById(int categoryId)
        {
            var categories = await _unitOfWork.Category.GetCategoryWithActivitiesByIdAsync(categoryId);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}
