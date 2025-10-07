using ActiviGo.Application.DTOs.ZoneDtos;
using ActiviGo.Domain.Models;
using System.Runtime.CompilerServices;

namespace ActiviGo.Application.Interfaces
{
    public interface IZoneService
    {
        Task<IEnumerable<ZoneReadDto>> GetAllZonesAsync();
        Task<ZoneReadDto> GetZoneById(int id, CancellationToken ct);
        Task<Zone> CreateZoneAsync(ZoneCreateDto zoneCreateDto, CancellationToken ct);
        Task<Zone> UpdateZoneAsync(int id, ZoneUpdateDto zoneUpdateDto, CancellationToken ct);
    }
}
