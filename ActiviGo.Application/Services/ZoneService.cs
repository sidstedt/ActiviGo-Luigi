using ActiviGo.Application.DTOs;
using ActiviGo.Application.DTOs.ZoneDtos;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ActiviGo.Application.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _repo;
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public ZoneService(IZoneRepository repo, IMapper mapper, IUnitofWork unitofWork)
        {
            _repo = repo;
            _mapper = mapper;
            _unitofWork = unitofWork;
        }

        public async Task<Zone> CreateZoneAsync(ZoneCreateDto zoneCreateDto, CancellationToken ct)
        {
            var createZone = _mapper.Map<Zone>(zoneCreateDto);

            var created = await _repo.CreateZoneAsync(createZone);

            return _mapper.Map<Zone>(created);
        }

        public async Task<IEnumerable<ZoneReadDto>> GetAllZonesAsync()
        {
            var zone = await _repo.GetAllZonesAsync();

            return _mapper.Map<IEnumerable<ZoneReadDto>>(zone);
        }

        public async Task<ZoneReadDto> GetZoneById(int id, CancellationToken ct)
        {
            var zoneId = _repo.FindByIdAsync(id);

            if (zoneId == null) throw new Exception($"Could not ifnd the id {id}");

            return _mapper.Map<ZoneReadDto>(zoneId);
        }

        public async Task<Zone> UpdateZoneAsync(int id, ZoneUpdateDto zoneUpdateDto, CancellationToken ct)
        {
            var zone = _repo.FindByIdAsync(id);

            if (zone == null) throw new Exception($"Could not ifnd the id {id}");

            await _mapper.Map(zoneUpdateDto, zone);

            //await _repo.UpdateZoneAsync(zone);

            await _unitofWork.SaveChangesAsync(); 

            return _mapper.Map<Zone>(zone);
        }
    }
}
