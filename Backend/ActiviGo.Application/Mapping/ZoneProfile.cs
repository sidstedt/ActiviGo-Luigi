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
                .ForMember(z => z.LocationName, opt => opt.MapFrom(scr => scr.Location.Name))
                 .ForMember(dest => dest.LocationName, opt =>
                         opt.MapFrom(src => src.Location.Name))
                 .ForMember(dest => dest.EnvironmentMessage, opt =>
                         opt.MapFrom(src => src.IsOutdoor
                        ? "Denna zon ligger utomhus"
                        : "Denna zon ligger inomhus"));


            CreateMap<ZoneDto, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsOutdoor, opt => opt.MapFrom(src => src.IsOutdoor))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId));

            CreateMap<Zone, ZoneDto>()
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsOutdoor, opt => opt.MapFrom(src => src.IsOutdoor))
                .ForMember(z => z.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                    .ForMember(z => z.ActivityName, opt => opt.MapFrom(src => src.Activities.Select(a => a.Name).FirstOrDefault()));

            CreateMap<CreateZoneDto, Zone>()
                            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                            .ForMember(dest => dest.IsOutdoor, opt => opt.MapFrom(src => src.IsOutdoor))
                            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId));

            CreateMap<ZoneUpdateDto, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsOutdoor, opt => opt.MapFrom(src => src.IsOutdoor))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId));
        }
    }
}
