using ActiviGo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Application.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> CreateActivityAsync(ActivityCreateDto activityCreateDto);
        Task<ICollection<ActivityResponseDto>> GetAllActivitiesAsync();
        Task<ActivityResponseDto?> GetActivityByIdAsync(int id);
        Task<ActivityResponseDto> UpdateActivityAsync(int id, ActivityUpdateDto activityUDto);
        Task<bool> DeleteActivityAsync(int id);

    }
}
