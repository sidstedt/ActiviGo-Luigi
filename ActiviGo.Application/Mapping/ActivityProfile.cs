using ActiviGo.Application.DTOs;
using ActiviGo.Application.DTOs.CategoryDtos;
using ActiviGo.Application.DTOs.ZoneDtos;
using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Models;
using AutoMapper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.Mapping
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
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
