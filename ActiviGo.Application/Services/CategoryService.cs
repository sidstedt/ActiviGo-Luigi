//using ActiviGo.Application.DTOs;
//using ActiviGo.Application.DTOs.CategoryDtos;
//using ActiviGo.Application.Interfaces;
//using ActiviGo.Application.Services;
//using ActiviGo.Domain.Interfaces;
//using ActiviGo.Domain.Models;
//using AutoMapper;
//using Microsoft.Extensions.Logging;

//public class CategoryService
//    : GenericService<Category, CategoryReadDto, CreateCategoryDto, CategoryUpdateDto>, ICategoryService
//{
//    private readonly IUnitofWork _unitOfWork;
//    private readonly IMapper _mapper;
//    private readonly ILogger _logger;
    
//    public CategoryService(IUnitofWork unitOfWork, IMapper mapper, ILogger logger)
//        : base(unitOfWork.Category, mapper)
//    {
//        _logger = logger;
//        _unitOfWork = unitOfWork;
//        _mapper = mapper;
//    }

//    public async Task<IEnumerable<Category>> GetCategoryWithActivitiesById(int categoryId, CancellationToken ct)
//    {
//        var findId = _mapper.Map<Category>(categoryId);
//        if (findId.Id == null) return null;

//        return 
//    }
//}
