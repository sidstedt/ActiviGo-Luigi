using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ActiviGo.Application.Services
{
    public class LocationService
        : GenericService<Location, LocationResponseDto, LocationCreateDto, LocationUpdateDto>,
          ILocationService
    {
        private readonly IUnitofWork _uow;
        private readonly ILogger<LocationService> _logger;
        private readonly IMapper _mapper;

        public LocationService(
            ILogger<LocationService> logger,
            IMapper mapper,
            IUnitofWork unitofWork
        ) : base(unitofWork.Location, mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _uow = unitofWork;
        }

        public override async Task<LocationResponseDto> CreateAsync(LocationCreateDto dto)
        {
            var location = new Location
            {
                Name = dto.Name,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Zones = new List<Zone>()
            };

            // Hämta Zone-entiteter baserat på ZoneIds
            if (dto.ZoneIds.Any())
            {
                var allZones = await _uow.Zone.GetAllAsync();
                var zones = allZones.Where(z => dto.ZoneIds.Contains(z.Id)).ToList();

                location.Zones = zones.ToList();
            }

            await _uow.Location.AddAsync(location);
            await _uow.SaveChangesAsync();

            return _mapper.Map<LocationResponseDto>(location);
        }


        public override async Task<LocationResponseDto?> UpdateAsync(int id, LocationUpdateDto dto)
        {
            var existing = await _uow.Location.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Location with ID {Id} not found", id);
                return null;
            }

            _mapper.Map(dto, existing);
            await _uow.Location.UpdateAsync(existing);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("Updated location with ID {Id}", id);

            return _mapper.Map<LocationResponseDto>(existing);
        }

        public override async Task<IEnumerable<LocationResponseDto>> GetAllAsync()
        {
            var locations = await _uow.Location.GetAllIncludingAsync(l => l.Zones);
            return _mapper.Map<IEnumerable<LocationResponseDto>>(locations);
        }

        public override async Task<LocationResponseDto?> GetByIdAsync(int id)
        {
            var location = await _uow.Location.GetByIdIncludingAsync(id, l => l.Zones);
            return location is null ? null : _mapper.Map<LocationResponseDto>(location);
        }

    }
}
