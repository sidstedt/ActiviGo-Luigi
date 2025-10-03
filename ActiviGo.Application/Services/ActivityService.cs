using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ActivityResponseDto> CreateActivityAsync(ActivityCreateDto activityDto)
        {
            var activityEntity = _mapper.Map<Activity>(activityDto);

            var createdActivity = await _repository.AddActivityAsync(activityEntity);

            return _mapper.Map<ActivityResponseDto>(createdActivity);
        }

        public async Task<ICollection<ActivityResponseDto>> GetAllActivitiesAsync()
        {
            var activities = await _repository.GetAllActivitiesAsync();

            return _mapper.Map<ICollection<ActivityResponseDto>>(activities);
        }

        public async Task<ActivityResponseDto?> GetActivityByIdAsync(int id)
        {
            var activity = await _repository.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return null;
            }

            return _mapper.Map<ActivityResponseDto>(activity);
        }

        public async Task<ActivityResponseDto> UpdateActivityAsync(int id, ActivityUpdateDto activityDto)
        {
            var existingActivity = await _repository.GetActivityByIdAsync(id);
            if (existingActivity == null)
            {
                throw new KeyNotFoundException($"Activity with Id {id} not found.");
            }

            _mapper.Map(activityDto, existingActivity);

            var updatedActivity = await _repository.UpdateActivityAsync(existingActivity);


            return _mapper.Map<ActivityResponseDto>(updatedActivity);
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {

            var isDeleted = await _repository.DeleteActivityAsync(id);

            if (!isDeleted)
            {
                throw new KeyNotFoundException($"Activity with Id {id} not found for deletion.");
            }
            return true;
        }
    }
}