using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Mapping
{
    public class ZoneProfile : Profile
    {
        public ZoneProfile()
        {
            CreateMap<Zone, ZoneResponseDto>();
        }
    }
}
