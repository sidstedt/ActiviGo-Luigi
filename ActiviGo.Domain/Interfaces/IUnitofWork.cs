using ActiviGo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviGo.Domain.Interfaces
{
    public interface IUnitofWork
    {
        IActivityRepository Activity { get; }
        //IActivityOccurence ActivityOccurence { get; }
        IBookingRepository Booking { get; }
        ICategoryRepository Category { get; }
        //IUserRepository User { get; }
        IZoneRepository Zone { get; }
        Task<int> SaveChangesAsync();
    }
}
