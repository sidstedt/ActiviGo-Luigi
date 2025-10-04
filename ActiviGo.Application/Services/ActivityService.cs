using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Services
{
    public class ActivityService
        : GenericService<Activity, ActivityResponseDto, ActivityCreateDto, ActivityUpdateDto>,
          IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository, IMapper mapper)
            : base(activityRepository, mapper)
        {
            _activityRepository = activityRepository;
        }

        // No specific methods for now, but can be added in the future
    }
}
