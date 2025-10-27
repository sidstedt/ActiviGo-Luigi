using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Mapping
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()//Acticity-mappings
        {
            CreateMap<ActivityCreateDto, Activity>();

            CreateMap<ActivityUpdateDto, Activity>();

            CreateMap<Activity, ActivityResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.Zone.Name))
                .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Staff != null ? $"{src.Staff.FirstName} {src.Staff.LastName}" : "Unassigned"));


           
        }
    }
}
