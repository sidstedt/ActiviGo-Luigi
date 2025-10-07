using ActiviGo.Application.DTOs;
using ActiviGo.Application.DTOs.CategoryDtos;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Services
{
    public class CategoryService : ICategoryService
    {
        //private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _repo;

        public CategoryService(IUnitofWork unitofWork, IMapper mapper, ICategoryRepository repo) 
        {
            //_unitofWork = unitofWork;   
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken ct)
        {
            var create =  _mapper.Map<Category>(createCategoryDto);

            var result = await _repo.CreateCategoryAsync(create);
             _mapper.Map<CategoryReadDto>(result);

            return result;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var delete = await _repo.DeleteCategoryAsync(categoryId);

            if (!delete) throw new Exception($"Category with id {categoryId} was not found");

            return true;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync()
        {
             var category = await _repo.GetAllCategoriesAsync();

            return _mapper.Map<IEnumerable<CategoryReadDto>>(category);
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId, CancellationToken ct)
        {
            var category = await _repo.FindByIdAsync(categoryId);

            if (category == null) throw new Exception($"Cannot find the category id {categoryId}");

            return _mapper.Map<Category>(category);
        }

        public async Task<Category?> GetCategoryWithActivities(int categoryId, CancellationToken ct)
        {
            var category = await _repo.GetCategoriesWithActivitiesAsync(categoryId);

            if (category is null)
                throw new Exception($"Category with Id {categoryId} was not found");

            return  _mapper.Map<Category>(category);
        }

        public async Task<CategoryReadDto> UpdateCategoryAsync(int id, CategoryUpdateDto updateCategoryDto, CancellationToken ct)
        {
            var update = await _repo.FindByIdAsync(id);

            if (update is null) throw new Exception($"Could not update {id} for some reason");

            _mapper.Map(updateCategoryDto, update);

           await _repo.UpdateCategoryAsync(update);

            return _mapper.Map<CategoryReadDto>(update);
        }
    }
}
