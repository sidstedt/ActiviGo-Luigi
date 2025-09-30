using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Data
{
    public class ActiviGoDbContext : DbContext
    {
        public ActiviGoDbContext(DbContextOptions<ActiviGoDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityOccurence> ActivityOccurences { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Activity ↔ Category
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Activity ↔ Zone
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Zone)
                .WithMany(z => z.Activities)
                .HasForeignKey(a => a.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            // Activity ↔ Staff (User)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Staff)
                .WithMany(u => u.StaffActivities)
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Activity ↔ ActivityOccurrences
            modelBuilder.Entity<ActivityOccurence>()
                .HasOne(o => o.Activity)
                .WithMany(a => a.Occurences)
                .HasForeignKey(o => o.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking ↔ User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking ↔ ActivityOccurrence
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ActivityOccurence)
                .WithMany(o => o.Bookings)
                .HasForeignKey(b => b.ActivityOccurenceId)
                .OnDelete(DeleteBehavior.Cascade);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // --- Users ---
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    IsActive = true,
                    UserName = "alice@test.com",
                    Email = "alice@test.com",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Smith",
                    IsActive = true,
                    UserName = "bob@test.com",
                    Email = "bob@test.com",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = 3,
                    FirstName = "Charlie",
                    LastName = "Brown",
                    IsActive = true,
                    UserName = "charlie@test.com",
                    Email = "charlie@test.com",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = 4,
                    FirstName = "Dana",
                    LastName = "Instructor",
                    IsActive = true,
                    UserName = "dana@test.com",
                    Email = "dana@test.com",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = 5,
                    FirstName = "Eli",
                    LastName = "Coach",
                    IsActive = true,
                    UserName = "eli@test.com",
                    Email = "eli@test.com",
                    EmailConfirmed = true
                },
                new User
                {
                    Id = 6,
                    FirstName = "Admin",
                    LastName = "User",
                    IsActive = true,
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    EmailConfirmed = true
                }
            );

            // --- Categories ---
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Training",
                    Description = "Physical training and exercise"
                },
                new Category
                {
                    Id = 2,
                    Name = "Outdoor Activities",
                    Description = "Activities outdoors"
                },
                new Category
                {
                    Id = 3,
                    Name = "Ball Sports",
                    Description = "Football, Tennis, etc."
                },
                new Category
                {
                    Id = 4,
                    Name = "Aquatics",
                    Description = "Swimming and water activities"
                },
                new Category
                {
                    Id = 5,
                    Name = "Wellness",
                    Description = "Yoga, meditation, etc."
                }
            );

            // --- Zones ---
            modelBuilder.Entity<Zone>().HasData(
                new Zone
                {
                    Id = 1,
                    Name = "Gym Hall",
                    Address = "Main Facility - Zone A",
                    Latitude = 59.33,
                    Longitude = 18.06,
                    InOut = ZoneType.Indoor
                },
                new Zone
                {
                    Id = 2,
                    Name = "Spinning Room",
                    Address = "Main Facility - Zone B",
                    Latitude = 59.33,
                    Longitude = 18.07,
                    InOut = ZoneType.Indoor
                },
                new Zone
                {
                    Id = 3,
                    Name = "Climbing Wall",
                    Address = "Main Facility - Zone C",
                    Latitude = 59.33,
                    Longitude = 18.08,
                    InOut = ZoneType.Indoor
                },
                new Zone
                {
                    Id = 4,
                    Name = "Tennis Court",
                    Address = "Main Facility - Zone D",
                    Latitude = 59.34,
                    Longitude = 18.05,
                    InOut = ZoneType.Outdoor
                },
                new Zone
                {
                    Id = 5,
                    Name = "Football Field",
                    Address = "Main Facility - Zone E",
                    Latitude = 59.34,
                    Longitude = 18.06,
                    InOut = ZoneType.Outdoor
                },
                new Zone
                {
                    Id = 6,
                    Name = "Swimming Pool",
                    Address = "Main Facility - Zone F",
                    Latitude = 59.35,
                    Longitude = 18.04,
                    InOut = ZoneType.Indoor
                },
                new Zone
                {
                    Id = 7,
                    Name = "Spa & Relax",
                    Address = "Main Facility - Zone G",
                    Latitude = 59.35,
                    Longitude = 18.05,
                    InOut = ZoneType.Indoor
                }
            );

            // --- Activities ---
            modelBuilder.Entity<Activity>().HasData(
                new Activity
                {
                    Id = 1,
                    Name = "Yoga",
                    Description = "Relaxing yoga session",
                    Price = 15,
                    MaxParticipants = 15,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 5,
                    ZoneId = 1,
                    StaffId = 4
                },
                new Activity
                {
                    Id = 2,
                    Name = "Pilates",
                    Description = "Core Pilates class",
                    Price = 15,
                    MaxParticipants = 15,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 5,
                    ZoneId = 1,
                    StaffId = 4
                },

                new Activity
                {
                    Id = 3,
                    Name = "Spinning",
                    Description = "High intensity spinning",
                    Price = 20,
                    MaxParticipants = 20,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 1,
                    ZoneId = 2,
                    StaffId = 5
                },
                new Activity
                {
                    Id = 4,
                    Name = "Football Practice",
                    Description = "Outdoor football training",
                    Price = 10,
                    MaxParticipants = 22,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 3,
                    ZoneId = 5,
                    StaffId = 5
                },
                new Activity
                {
                    Id = 5,
                    Name = "Tennis Practice",
                    Description = "Tennis training session",
                    Price = 12,
                    MaxParticipants = 8,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 3,
                    ZoneId = 4,
                    StaffId = 5
                },
                new Activity
                {
                    Id = 6,
                    Name = "Climbing",
                    Description = "Indoor climbing",
                    Price = 18,
                    MaxParticipants = 10,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 2,
                    ZoneId = 3,
                    StaffId = 4
                },
                new Activity
                {
                    Id = 7,
                    Name = "Swimming Training",
                    Description = "Lap swimming session",
                    Price = 15,
                    MaxParticipants = 12,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 4,
                    ZoneId = 6,
                    StaffId = 5
                },
                new Activity
                {
                    Id = 8,
                    Name = "Water Aerobics",
                    Description = "Fun aquatic exercise",
                    Price = 15,
                    MaxParticipants = 15,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 4,
                    ZoneId = 6,
                    StaffId = 4
                },
                new Activity
                {
                    Id = 9,
                    Name = "Meditation",
                    Description = "Mindfulness meditation",
                    Price = 10,
                    MaxParticipants = 15,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 5,
                    ZoneId = 7,
                    StaffId = 4
                },
                new Activity
                {
                    Id = 10,
                    Name = "Athletics",
                    Description = "Track and field practice",
                    Price = 12,
                    MaxParticipants = 20,
                    IsActive = true,
                    IsPrivate = false,
                    IsAvailable = true,
                    CategoryId = 2,
                    ZoneId = 5,
                    StaffId = 5
                }
            );

            // --- ActivityOccurrences ---
            modelBuilder.Entity<ActivityOccurence>().HasData(
                new ActivityOccurence
                {
                    Id = 1,
                    ActivityId = 1,
                    ZoneId = 1,
                    StartTime = new DateTime(2025, 10, 1, 8, 0, 0),
                    EndTime = new DateTime(2025, 10, 1, 9, 0, 0),
                    DurationMinutes = 60
                },
                new ActivityOccurence
                {
                    Id = 2,
                    ActivityId = 2,
                    ZoneId = 1,
                    StartTime = new DateTime(2025, 10, 2, 10, 0, 0),
                    EndTime = new DateTime(2025, 10, 2, 11, 0, 0),
                    DurationMinutes = 60
                },
                new ActivityOccurence
                {
                    Id = 3,
                    ActivityId = 3,
                    ZoneId = 2,
                    StartTime = new DateTime(2025, 10, 3, 18, 0, 0),
                    EndTime = new DateTime(2025, 10, 3, 19, 0, 0),
                    DurationMinutes = 60
                }
            );
        }

    }
}


