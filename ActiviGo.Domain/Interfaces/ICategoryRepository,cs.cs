using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesWithActivitiesAsync(int id);
    }
}
