using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

using Activity = ActiviGo.Domain.Models.Activity;


namespace ActiviGo.Domain.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity> AddActivityAsync(Activity activity);
        Task<ICollection<Activity>> GetAllActivitiesAsync();
        Task<Activity?> GetActivityByIdAsync(int id);
        Task<Activity> UpdateActivityAsync(Activity activity);
        Task<bool> DeleteActivityAsync(int id);
    }
}