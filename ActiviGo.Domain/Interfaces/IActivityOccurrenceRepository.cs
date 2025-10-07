using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface IActivityOccurrenceRepository
    : IGenericRepository<ActivityOccurrence>
    {
        Task<bool> CheckZoneAvailabilityAsync(int zoneId, DateTime startTime, DateTime endTime);
        Task<int> GetCurrentParticipantCountAsync(int occurrenceId);
        Task<ICollection<ActivityOccurrence>> GetOccurrencesByActivityIdAsync(int activityId);
        Task<ActivityOccurrence?> GetActivityOccurrenceByIdAsync(int activityOccurrenceId, CancellationToken ct);
    }

}
