using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface IActivityRepository : IGenericRepository<Activity> //Repository interface for Activity entity.
    {
        Task<IEnumerable<Activity>> GetByStaffAsync(Guid staffId, CancellationToken ct);
    }
}
