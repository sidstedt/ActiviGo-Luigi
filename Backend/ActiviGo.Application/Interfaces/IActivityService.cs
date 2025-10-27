using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface IActivityService
        : IGenericService<Activity, ActivityResponseDto, ActivityCreateDto, ActivityUpdateDto>
    {
        Task<IEnumerable<ActivityResponseDto>> GetByStaffAsync(Guid staffId, CancellationToken ct);
    }
}
