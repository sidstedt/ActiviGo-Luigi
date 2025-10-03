using ActiviGo.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiviGo.Application.Interfaces
{
    public interface IActivityOccurrenceService
    {
        Task<ActivityOccurrenceResponseDto> CreateOccurrenceAsync(ActivityOccurrenceCreateDto createDto);
        Task<ICollection<ActivityOccurrenceResponseDto>> GetAllOccurrencesAsync();
        Task<ActivityOccurrenceResponseDto?> GetOccurrenceByIdAsync(int id);
        Task<ActivityOccurrenceResponseDto> UpdateOccurrenceAsync(int id, ActivityOccurrenceUpdateDto updateDto);
        Task<bool> DeleteOccurrenceAsync(int id);
        Task<ICollection<ActivityOccurrenceResponseDto>> GetAvailableOccurrencesByActivityIdAsync(int activityId);
    }
}
