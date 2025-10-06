using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface IActivityService
        : IGenericService<Activity, ActivityResponseDto, ActivityCreateDto, ActivityUpdateDto>
    {
        // Empty for now, but can be extended with activity-specific methods in the future
    }
}
