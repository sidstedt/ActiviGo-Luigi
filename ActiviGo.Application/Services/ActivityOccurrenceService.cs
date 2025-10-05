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
        private readonly IActivityOccurrenceRepository _occurrenceRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly ILogger<ActivityOccurrenceService> _logger;

        public ActivityOccurrenceService(
            IActivityOccurrenceRepository occurrenceRepository,
            IActivityRepository activityRepository,
            IMapper mapper,
            ILogger<ActivityOccurrenceService> logger)
            : base(occurrenceRepository, mapper)
        {
            _occurrenceRepository = occurrenceRepository;
            _activityRepository = activityRepository;
            _logger = logger;
        }

        public override async Task<ActivityOccurrenceResponseDto> CreateAsync(ActivityOccurrenceCreateDto dto)
        {
            var activity = await _activityRepository.GetByIdAsync(dto.ActivityId);
            if (activity == null)
            {
                _logger.LogWarning("Försök att skapa ActivityOccurrence för icke-existerande ActivityId: {ActivityId}", dto.ActivityId);
                throw new KeyNotFoundException($"Activity med ID {dto.ActivityId} hittades inte.");
            }

            int durationMinutes = dto.DurationMinutes ?? activity.DurationMinutes;
            int zoneId = dto.ZoneId ?? activity.ZoneId;
            var endTime = dto.StartTime.AddMinutes(durationMinutes);

            bool isZoneAvailable = await _occurrenceRepository.CheckZoneAvailabilityAsync(zoneId, dto.StartTime, endTime);
            if (!isZoneAvailable)
            {
                throw new InvalidOperationException($"Zonen (ID: {zoneId}) är redan bokad under den begärda tidsperioden.");
            }

            var occurrence = _mapper.Map<ActivityOccurrence>(dto);
            occurrence.DurationMinutes = durationMinutes;
            occurrence.EndTime = endTime;
            occurrence.ZoneId = zoneId;

            await _occurrenceRepository.AddAsync(occurrence);
            var fullOccurrence = await _occurrenceRepository.GetByIdAsync(occurrence.Id);

            return _mapper.Map<ActivityOccurrenceResponseDto>(fullOccurrence);
        }

        public async Task<ICollection<ActivityOccurrenceResponseDto>> GetAvailableOccurrencesByActivityIdAsync(int activityId)
        {
            var occurrences = await _occurrenceRepository.GetOccurrencesByActivityIdAsync(activityId);
            return _mapper.Map<ICollection<ActivityOccurrenceResponseDto>>(occurrences);
        }
    }
}