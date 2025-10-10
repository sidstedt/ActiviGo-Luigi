using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using System.Runtime.CompilerServices;

namespace ActiviGo.Application.Interfaces
{
    public interface IZoneService : IGenericService<Zone, ZoneReadDto, ZoneDto,  ZoneUpdateDto>
    {
        Task<IEnumerable<ZoneReadDto>> GetZonesWithActivititesByIdAsync(int id);
        Task<IEnumerable<ZoneDto>> GetZonesByLocationId(int locationId);
        Task<IEnumerable<ZoneReadDto>> GetAllZonesWithRelations();
        Task AddActivityToZone(int zoneId, int activityId);
        Task RemoveActivityFromZone(int zoneId, int activityId);
        Task<ZoneReadDto> CreateAsync(CreateZoneDto dto);
    }
}
