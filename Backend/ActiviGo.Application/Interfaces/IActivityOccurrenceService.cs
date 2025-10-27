using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface IActivityOccurrenceService
        : IGenericService<ActivityOccurrence, ActivityOccurrenceResponseDto, ActivityOccurrenceCreateDto, ActivityOccurrenceUpdateDto>
    {
        Task<ICollection<ActivityOccurrenceResponseDto>> GetAvailableOccurrencesByActivityIdAsync(int activityId);

        Task<IEnumerable<ActivityOccurrenceResponseDto>> GetByStaffAsync(Guid staffId, DateTime? from, DateTime? to, CancellationToken ct);
    }

}
