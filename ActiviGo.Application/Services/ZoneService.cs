using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ActiviGo.Application.Services
{
    public class ZoneService : GenericService<Zone, ZoneReadDto, ZoneDto, ZoneUpdateDto>, IZoneService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public ZoneService(ILogger<ZoneService> logger, IMapper mapper, IUnitofWork unitofWork) : base(unitofWork.Zone, mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public Task AddActivityToZone(int zoneId, int activityId)
        {
            return _unitofWork.Zone.AddActivityToZoneAsync(zoneId, activityId);
        }

        public async Task<IEnumerable<ZoneReadDto>> GetAllZonesWithRelations()
        {
            var zone = await _unitofWork.Zone.GetAllZonesWithActivitiesAndLocationAsync();
            return _mapper.Map<IEnumerable<ZoneReadDto>>(zone);
        }

        public async Task<IEnumerable<ZoneDto>> GetZonesByLocationId(int locationId)
        {
            var zone = await _unitofWork.Zone.GetZonesByLocationIdAsync(locationId);
            return _mapper.Map<IEnumerable<ZoneDto>>(zone);
        }

        public async Task<IEnumerable<ZoneReadDto>> GetZonesWithActivititesByIdAsync(int id)
        {
            var zones = await _unitofWork.Zone.GetZoneWithActivitiesAsync(id);
            return _mapper.Map<IEnumerable<ZoneReadDto>>(zones);
        }

        public async Task RemoveActivityFromZone(int zoneId, int activityId)
        {
            await _unitofWork.Zone.RemoveActivityFromZoneAsync(zoneId, activityId);
        }
    }
}
