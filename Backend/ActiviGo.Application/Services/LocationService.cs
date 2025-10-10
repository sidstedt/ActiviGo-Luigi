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
        private readonly IGeocodingService _geocodingService;

        public LocationService(
            ILogger<LocationService> logger,
            IMapper mapper,
            IUnitofWork unitofWork,
            IGeocodingService geocodingService)
            : base(unitofWork.Location, mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _uow = unitofWork;
            _geocodingService = geocodingService;
        }

        public override async Task<LocationResponseDto> CreateAsync(LocationCreateDto dto)
        {
            // Steg 1: Beräkna lat/long automatiskt om de saknas
            if ((!dto.Latitude.HasValue || !dto.Longitude.HasValue) && !string.IsNullOrWhiteSpace(dto.Address))
            {
                var coords = await _geocodingService.GetCoordinatesFromAddressAsync(dto.Address);
                if (coords != null)
                {
                    dto.Latitude = coords.Value.Latitude;
                    dto.Longitude = coords.Value.Longitude;
                }
                else
                {
                    _logger.LogWarning("Kunde inte hitta koordinater för adressen: {Address}", dto.Address);
                }
            }

            // Steg 2: Skapa Location
            var location = new Location
            {
                Name = dto.Name,
                Address = dto.Address,
                Latitude = dto.Latitude ?? 0,
                Longitude = dto.Longitude ?? 0
            };

            // Steg 3: Koppla zoner
            if (dto.ZoneIds.Any())
            {
                var allZones = await _uow.Zone.GetAllAsync();
                var zones = allZones.Where(z => dto.ZoneIds.Contains(z.Id)).ToList();
                location.Zones = zones;
            }

            // Steg 4: Spara i DB
            await _uow.Location.AddAsync(location);
            await _uow.SaveChangesAsync();

            return _mapper.Map<LocationResponseDto>(location);
        }
        
        public override async Task<LocationResponseDto?> UpdateAsync(int id, LocationUpdateDto dto)
        {
            // Hämta location inklusive zoner
            var existing = await _uow.Location.GetByIdIncludingAsync(id, l => l.Zones);
            if (existing == null)
            {
                _logger.LogWarning("Location with ID {Id} not found", id);
                return null;
            }

            // Steg 1: Beräkna lat/long automatiskt om de saknas
            if ((!dto.Latitude.HasValue || !dto.Longitude.HasValue) && !string.IsNullOrWhiteSpace(dto.Address))
            {
                var coords = await _geocodingService.GetCoordinatesFromAddressAsync(dto.Address);
                if (coords != null)
                {
                    dto.Latitude = coords.Value.Latitude;
                    dto.Longitude = coords.Value.Longitude;
                }
                else
                {
                    _logger.LogWarning("Kunde inte hitta koordinater för adressen: {Address}", dto.Address);
                }
            }

            // Steg 2: Mappa övriga fält
            _mapper.Map(dto, existing);

            // Steg 3: Lägg till nya zoner utan att ta bort befintliga
            if (dto.ZoneIds != null && dto.ZoneIds.Any())
            {
                var allZones = await _uow.Zone.GetAllAsync();

                // Hitta zoner som ska läggas till
                var toAdd = allZones
                    .Where(z => dto.ZoneIds.Contains(z.Id) && !existing.Zones.Any(ez => ez.Id == z.Id))
                    .ToList();

                foreach (var zone in toAdd)
                    existing.Zones.Add(zone);
            }

            // Steg 4: Spara ändringar
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
