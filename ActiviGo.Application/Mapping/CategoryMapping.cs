using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;
using System.Linq;

namespace ActiviGo.Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.Activities != null && src.Activities.Any() ? src.Activities.First().Id : 0))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.Activities.Select(a => a.Name)));
           
            CreateMap<Category, CategoryWithActivitiesDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities));

            // mapping for actititys in category
            CreateMap<Activity, ActivityForCategoryDto>()
                .ForMember(a => a.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Category, CreateCategoryDto>();
        }
    }
}
