using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;
using System.Linq;

namespace ActiviGo.Application.Mapping
{
    public class ZoneProfile : Profile
    {
        public ZoneProfile()
        {
            CreateMap<Zone, ZoneResponseDto>();
            CreateMap<Zone, ZoneReadDto>()
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.Activities.Select(a => a.Id)))
                .ForMember(z => z.LocationId, opt => opt.MapFrom(scr => scr.Location.Id));
        }
    }
}
