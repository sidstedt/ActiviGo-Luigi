using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Mapping
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            // Från DTO till Entity
            CreateMap<LocationCreateDto, Location>();
            CreateMap<LocationUpdateDto, Location>();

            // Från Entity till DTO
            CreateMap<Location, LocationResponseDto>()
                .ForMember(dest => dest.Zones, opt => opt.MapFrom(src => src.Zones));
        }
    }
}
