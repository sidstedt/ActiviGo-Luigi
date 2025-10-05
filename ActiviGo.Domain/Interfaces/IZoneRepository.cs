using ActiviGo.Domain.Models;
using System.Threading.Tasks;

namespace ActiviGo.Domain.Interfaces
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Zone>> GetAllZonesAsync();
        Task<IEnumerable<Zone>> GetZoneWithActivitiesAsync(string activityName);
        Task<Zone> CreateZoneAsync(Zone zone);
        Task<Zone> UpdateZoneAsync(Zone zone);
        Task <bool>DeleteZoneAsync(int ZoneId);
    }
}
