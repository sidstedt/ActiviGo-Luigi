using ActiviGo.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ActiviGo.Domain.Interfaces
{
    public interface IActivityOccurrenceRepository
    {
        Task<ActivityOccurrence> AddOccurrenceAsync(ActivityOccurrence occurrence);
        Task<ICollection<ActivityOccurrence>> GetAllOccurrencesAsync();
        Task<ActivityOccurrence> GetOccurrenceByIdAsync(int id);
        Task<ActivityOccurrence> UpdateOccurrenceAsync(ActivityOccurrence occurrence);
        Task<bool> DeleteOccurrenceAsync(int id);
        Task<bool> CheckZoneAvailabilityAsync(int zoneId, DateTime startTime, DateTime endTime);
        Task<int> GetCurrentParticipantCountAsync(int occurrenceId);
    }
}
