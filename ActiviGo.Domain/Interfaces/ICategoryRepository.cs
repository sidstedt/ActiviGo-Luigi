using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoryWithActivitiesByIdAsync(int id);

    }
}
