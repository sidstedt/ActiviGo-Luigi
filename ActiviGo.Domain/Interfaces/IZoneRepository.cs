using ActiviGo.Domain.Models;
using System.Threading.Tasks;

namespace ActiviGo.Domain.Interfaces
{
    public interface IZoneRepository : IGenericRepository<Zone>
    {
        Task<IEnumerable<Zone>> GetZoneWithActivitiesAsync(int id);

        //fetch all the zone for the given location id
        Task<IEnumerable<Zone>> GetZonesByLocationIdAsync(int locationId);

        //fetch all zones with activites and location details
        Task<IEnumerable<Zone>> GetAllZonesWithActivitiesAndLocationAsync();

        // add activity to a zone
        Task AddActivityToZoneAsync(int zoneId, int activityId);

        // remove activity from a zone
        Task RemoveActivityFromZoneAsync(int zoneId, int activityId);
    }
}
