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
        private readonly IUnitofWork _uow;

        public ActivityService(IUnitofWork uow, IMapper mapper)
            : base(uow.Activity, mapper)
        {
            _uow = uow;
        }

        // No specific methods for now, but can be added in the future
    }
}
