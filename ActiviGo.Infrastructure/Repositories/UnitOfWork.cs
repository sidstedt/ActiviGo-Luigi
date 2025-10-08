using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using System;

namespace ActiviGo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitofWork, IDisposable
    {
        private readonly ActiviGoDbContext _context;
        private IActivityRepository _activity;
        private IActivityOccurrenceRepository _activityOccurrence;
        private IBookingRepository _booking;
        private ICategoryRepository _category;
        private ILocationRepository _location;
        //private IUserRepository _user;
        private IZoneRepository _zone;

        public UnitOfWork(ActiviGoDbContext context)
        {
            _context = context;
        }

        public IActivityRepository Activity => _activity ??= new ActivityRepository(_context);

        public IActivityOccurrenceRepository ActivityOccurrence => _activityOccurrence ??= new ActivityOccurrenceRepository(_context);

        public IBookingRepository Booking => _booking ??= new BookingRepository(_context);

        public ICategoryRepository Category => _category ??= new CategoryRepository(_context);

        //public IUserRepository => _user ??= new UserRepository(_context);

        public IZoneRepository Zone => _zone ??= new ZoneRepository(_context);
        public ILocationRepository Location => _location ??= new LocationRepository(_context);



        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
