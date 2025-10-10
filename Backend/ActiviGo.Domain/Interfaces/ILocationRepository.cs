using ActiviGo.Domain.Models;
using System.Linq.Expressions;

namespace ActiviGo.Domain.Interfaces
{
    public interface ILocationRepository
    : IGenericRepository<Location>
    {
        Task<IEnumerable<Location>> GetAllIncludingAsync(params Expression<Func<Location, object>>[] includes);
        Task<Location?> GetByIdIncludingAsync(int id, params Expression<Func<Location, object>>[] includes);

    }
}
