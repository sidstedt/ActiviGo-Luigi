using ActiviGo.Application.DTOs;
using ActiviGo.Application.DTOs.CategoryDtos;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping() 
        {
            // CATEGORIES
            CreateMap<Category, CategoryReadDto>();
            CreateMap<Category, CreateCategoryDto>();

            //Zone
            //CreateMap<ZoneUpdateDto, Zone>()
            //.ForMember(dest => dest.InOut,
            //opt => opt.MapFrom(src => Enum.Parse<ZoneType>(src.InOut, true)));
        }
    }
}
