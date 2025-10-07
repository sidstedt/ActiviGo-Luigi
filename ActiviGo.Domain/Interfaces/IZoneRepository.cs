using ActiviGo.Domain.Models;
using System.Threading.Tasks;

namespace ActiviGo.Domain.Interfaces
{
    public interface IZoneRepository : IGenericRepository<Zone>
    {
        Task<IEnumerable<Zone>> GetZoneWithActivitiesAsync(int id);
    }
}
