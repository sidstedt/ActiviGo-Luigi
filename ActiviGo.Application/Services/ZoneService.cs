using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ActiviGo.Application.Services
{
    public class ZoneService : GenericService<Zone, ZoneReadDto, ZoneDto, ZoneUpdateDto>, IZoneService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<ZoneService> _logger;

        public ZoneService(ILogger<ZoneService> logger, IMapper mapper, IUnitofWork unitofWork) : base(unitofWork.Zone, mapper)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        public override async Task<ZoneReadDto> CreateAsync(ZoneDto dto)
        {
            var createdZone = _mapper.Map<Zone>(dto);

            await _unitofWork.Zone.AddAsync(createdZone);
            await _unitofWork.SaveChangesAsync();

            return _mapper.Map<ZoneReadDto>(createdZone);
        }

        public async Task<IEnumerable<ZoneReadDto>> GetZonesWithActivititesByIdAsync(int id)
        {
            var zones = await _unitofWork.Zone.GetZoneWithActivitiesAsync(id);
            return _mapper.Map<IEnumerable<ZoneReadDto>>(zones);
        }

        public async Task<ZoneReadDto?> UpdateAsync(int id, ZoneReadDto dto)
        {
            var updateZone = await _unitofWork.Zone.GetByIdAsync(id);

            if (updateZone != null)  return null;

            _mapper.Map(dto, updateZone);
            _mapper.Map<ZoneUpdateDto>(updateZone);

            await _unitofWork.SaveChangesAsync();
            return _mapper.Map<ZoneReadDto?>(updateZone);
        }
    }
}
