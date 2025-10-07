using ActiviGo.Application.DTOs.ZoneDtos;
using ActiviGo.Domain.Models;
using System.Runtime.CompilerServices;

namespace ActiviGo.Application.Interfaces
{
    public interface IZoneService : IGenericService<Zone, ZoneReadDto, ZoneCreateDto,  ZoneUpdateDto>
    {
        Task<IEnumerable<ZoneReadDto>> GetZonesWithActivititesByIdAsync(int id);
    
    }
}
