using ActiviGo.Domain.Models;

namespace ActiviGo.Domain.Interfaces
{
    public interface IActivityRepository : IGenericRepository<Activity>
    {
        // staff scope
        Task<IEnumerable<Activity>> GetByStaffAsync(Guid staffId, CancellationToken ct);
    }
}
