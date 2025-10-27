using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface IActivityOccurrenceRepository //IRepository for ActivityOccurrence
    : IGenericRepository<ActivityOccurrence>
    {
        Task<bool> CheckZoneAvailabilityAsync(int zoneId, DateTime startTime, DateTime endTime);
        Task<int> GetCurrentParticipantCountAsync(int occurrenceId);
        Task<ICollection<ActivityOccurrence>> GetOccurrencesByActivityIdAsync(int activityId);
        Task<ActivityOccurrence?> GetActivityOccurrenceByIdAsync(int activityOccurrenceId, CancellationToken ct);

        Task<IEnumerable<ActivityOccurrence>> GetByStaffAsync(Guid staffId, DateTime? from, DateTime? to, CancellationToken ct);
    }

}
