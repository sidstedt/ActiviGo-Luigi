using ActiviGo.Domain.Models;
using System.Threading.Tasks;

namespace ActiviGo.Domain.Interfaces
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Zone>> GetAllCategoriesAsync();
        Task<IEnumerable<Zone>> GetCategoriesWithActivitiesAsync(string activityName);
        Task<Category> CreateCategoryAsync(Zone zone);
        Task<Category> UpdateCategoryAsync(Zone zone);
        Task DeleteCategoryAsync(int ZoneId);
    }
}
