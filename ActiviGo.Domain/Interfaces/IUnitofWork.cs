namespace ActiviGo.Domain.Interfaces
{
    public interface IUnitofWork
    {
        IActivityRepository Activity { get; }
        IActivityOccurrenceRepository ActivityOccurrence { get; }
        IBookingRepository Booking { get; }
        ICategoryRepository Category { get; }
        //IUserRepository User { get; }
        IZoneRepository Zone { get; }
        Task<int> SaveChangesAsync();
    }
}
