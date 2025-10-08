using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface IActivityService
        : IGenericService<Activity, ActivityResponseDto, ActivityCreateDto, ActivityUpdateDto>
    {
        // staff scope
        Task<IEnumerable<ActivityResponseDto>> GetByStaffAsync(Guid staffId, CancellationToken ct);
    }
}
