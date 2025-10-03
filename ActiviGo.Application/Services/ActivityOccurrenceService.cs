using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Använd den korrekta stavningen (eller den du valt att standardisera)
using ActivityOccurrence = ActiviGo.Domain.Models.ActivityOccurrence;

namespace ActiviGo.Application.Services
{
    public class ActivityOccurrenceService : IActivityOccurrenceService
    {
        private readonly IActivityOccurrenceRepository _occurrenceRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ActivityOccurrenceService> _logger;

        public ActivityOccurrenceService(
            IActivityOccurrenceRepository occurrenceRepository,
            IActivityRepository activityRepository,
            IMapper mapper,
            ILogger<ActivityOccurrenceService> logger)
        {
            _occurrenceRepository = occurrenceRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActivityOccurrenceResponseDto> CreateOccurrenceAsync(ActivityOccurrenceCreateDto dto)
        {

            var activity = await _activityRepository.GetActivityByIdAsync(dto.ActivityId);
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

            var createdOccurrence = await _occurrenceRepository.AddOccurrenceAsync(occurrence);

            var fullOccurrence = await _occurrenceRepository.GetOccurrenceByIdAsync(createdOccurrence.Id);

            return _mapper.Map<ActivityOccurrenceResponseDto>(fullOccurrence);
        }


        public async Task<ICollection<ActivityOccurrenceResponseDto>> GetAllOccurrencesAsync()
        {
            var occurrences = await _occurrenceRepository.GetAllOccurrencesAsync();
            return _mapper.Map<ICollection<ActivityOccurrenceResponseDto>>(occurrences);
        }

        public async Task<ActivityOccurrenceResponseDto?> GetOccurrenceByIdAsync(int id)
        {
            var occurrence = await _occurrenceRepository.GetOccurrenceByIdAsync(id);
            return occurrence == null ? null : _mapper.Map<ActivityOccurrenceResponseDto>(occurrence);
        }

        public async Task<ActivityOccurrenceResponseDto> UpdateOccurrenceAsync(int id, ActivityOccurrenceUpdateDto dto)
        {
            var existingOccurrence = await _occurrenceRepository.GetOccurrenceByIdAsync(id);
            if (existingOccurrence == null)
            {
                throw new KeyNotFoundException($"ActivityOccurrence med ID {id} hittades inte.");
            }

            _mapper.Map(dto, existingOccurrence);

            bool isZoneAvailable = await _occurrenceRepository.CheckZoneAvailabilityAsync(dto.ZoneId, dto.StartTime, dto.EndTime);

            existingOccurrence.EndTime = existingOccurrence.StartTime.AddMinutes(existingOccurrence.DurationMinutes);


            await _occurrenceRepository.UpdateOccurrenceAsync(existingOccurrence);

            var updatedOccurrence = await _occurrenceRepository.GetOccurrenceByIdAsync(id);

            return _mapper.Map<ActivityOccurrenceResponseDto>(updatedOccurrence);
        }

        public async Task<bool> DeleteOccurrenceAsync(int id)
        {

            return await _occurrenceRepository.DeleteOccurrenceAsync(id);
        }

        public Task<ICollection<ActivityOccurrenceResponseDto>> GetAvailableOccurrencesByActivityIdAsync(int activityId)
        {
            throw new NotImplementedException();
        }
    }
}