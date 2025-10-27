using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Services
{
    public class ActivityOccurrenceService
        : GenericService<ActivityOccurrence, ActivityOccurrenceResponseDto, ActivityOccurrenceCreateDto, ActivityOccurrenceUpdateDto>,
          IActivityOccurrenceService
    {
        private readonly IUnitofWork _uow;
        private readonly IActivityOccurrenceRepository _occurrenceRepository;
        private readonly ILogger<ActivityOccurrenceService> _logger;

        public ActivityOccurrenceService(
            IActivityOccurrenceRepository occurrenceRepository,
            IUnitofWork uow,
            IMapper mapper,
            ILogger<ActivityOccurrenceService> logger)
            : base(uow.ActivityOccurrence, mapper) // Call the base class constructor, passing the specific repository from the UoW and the mapper.
        {
            _occurrenceRepository = occurrenceRepository;
            _uow = uow;
            _logger = logger;
        }

        public override async Task<ActivityOccurrenceResponseDto> CreateAsync(ActivityOccurrenceCreateDto dto)
        {
            var activity = await _uow.Activity.GetByIdAsync(dto.ActivityId);// Validate Activity Existence
            if (activity == null)
            {
                _logger.LogWarning("Försök att skapa ActivityOccurrence för icke-existerande ActivityId: {ActivityId}", dto.ActivityId);
                throw new KeyNotFoundException($"Activity med ID {dto.ActivityId} hittades inte.");
            }

            int durationMinutes = dto.DurationMinutes ?? activity.DurationMinutes;//Determine Duration and Zone
            int zoneId = dto.ZoneId ?? activity.ZoneId;
            var endTime = dto.StartTime.AddMinutes(durationMinutes);

            bool isZoneAvailable = await _occurrenceRepository.CheckZoneAvailabilityAsync(zoneId, dto.StartTime, endTime);// Check Zone Availability
            if (!isZoneAvailable)
            {
                throw new InvalidOperationException($"Zonen (ID: {zoneId}) är redan bokad under den begärda tidsperioden.");
            }

            var occurrence = _mapper.Map<ActivityOccurrence>(dto);//Map and Prepare for Persistence
            occurrence.DurationMinutes = durationMinutes;
            occurrence.EndTime = endTime;
            occurrence.ZoneId = zoneId;

            await _occurrenceRepository.AddAsync(occurrence);//Persist and Retrieve
            var fullOccurrence = await _occurrenceRepository.GetByIdAsync(occurrence.Id);

            return _mapper.Map<ActivityOccurrenceResponseDto>(fullOccurrence);//Return Result
        }

        public async Task<ICollection<ActivityOccurrenceResponseDto>> GetAvailableOccurrencesByActivityIdAsync(int activityId)//Retrieves all activity occurrences for a specific activity ID.
        {
            var occurrences = await _occurrenceRepository.GetOccurrencesByActivityIdAsync(activityId);
            return _mapper.Map<ICollection<ActivityOccurrenceResponseDto>>(occurrences);
        }

        public async Task<IEnumerable<ActivityOccurrenceResponseDto>> GetByStaffAsync(Guid staffId, DateTime? from, DateTime? to, CancellationToken ct)//Retrieves activity occurrences assigned to a specific staff member within a given time range.
        {
            var list = await _occurrenceRepository.GetByStaffAsync(staffId, from, to, ct);
            return _mapper.Map<IEnumerable<ActivityOccurrenceResponseDto>>(list);
        }
    }
}