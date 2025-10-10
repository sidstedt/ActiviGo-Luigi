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

        // staff scope
        public async Task<IEnumerable<ActivityResponseDto>> GetByStaffAsync(Guid staffId, CancellationToken ct)
        {
            var activities = await _uow.Activity.GetByStaffAsync(staffId, ct);
            return _mapper.Map<IEnumerable<ActivityResponseDto>>(activities);
        }
    }
}
