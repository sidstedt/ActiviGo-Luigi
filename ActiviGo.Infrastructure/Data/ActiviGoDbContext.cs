using ActiviGo.Domain.Enum;
using ActiviGo.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Data
{
    public class ActiviGoDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ActiviGoDbContext(DbContextOptions<ActiviGoDbContext> options) : base(options) { }

        //public DbSet<User> Users { get; set; } // Redundant eftersom IdentityDbContext redan har detta
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityOccurrence> ActivityOccurrences { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User timestamps
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Activity decimal precision
            modelBuilder.Entity<Activity>()
                .Property(a => a.Price)
                .HasPrecision(10, 2); // avoids truncation warnings

            // Unik index för att hindra dubbelbokning (User + Occurrence)
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.UserId, b.ActivityOccurrenceId })
                .IsUnique();

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

            // Activity ↔ Staff
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Staff)
                .WithMany(u => u.StaffActivities)
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            // Activity ↔ Occurrences
            modelBuilder.Entity<ActivityOccurrence>()
                .HasOne(o => o.Activity)
                .WithMany(a => a.Occurrences)
                .HasForeignKey(o => o.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking ↔ User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking ↔ Occurrence
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ActivityOccurrence)
                .WithMany(o => o.Bookings)
                .HasForeignKey(b => b.ActivityOccurrenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Zone>()
                .HasMany(z => z.Activities)
                .WithOne(a => a.Zone)
                .HasForeignKey(a => a.ZoneId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Zones)
                .WithOne(z => z.Location)
                .HasForeignKey(z => z.LocaitonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityOccurrence>()
                .HasOne(o => o.Zone)
                .WithMany(z => z.ActivityOccurrences)
                .HasForeignKey(o => o.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // ---------------------------
            // Skapa roller
            // ---------------------------
            var userRoleId = Guid.NewGuid();
            var staffRoleId = Guid.NewGuid();
            var adminRoleId = Guid.NewGuid();

            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = userRoleId, Name = "User", NormalizedName = "USER" },
                new IdentityRole<Guid> { Id = staffRoleId, Name = "Staff", NormalizedName = "STAFF" },
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" }
            );

            // ---------------------------
            // Skapa användare
            // ---------------------------
            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            var staffId = Guid.NewGuid();
            var adminId = Guid.NewGuid();

            var hasher = new PasswordHasher<User>();

            var user1 = new User
            {
                Id = user1Id,
                UserName = "user1@example.com",
                NormalizedUserName = "USER1@EXAMPLE.COM",
                Email = "user1@example.com",
                NormalizedEmail = "USER1@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "One",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            user1.PasswordHash = hasher.HashPassword(user1, "Password123!");

            var user2 = new User
            {
                Id = user2Id,
                UserName = "user2@example.com",
                NormalizedUserName = "USER2@EXAMPLE.COM",
                Email = "user2@example.com",
                NormalizedEmail = "USER2@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "Two",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            user2.PasswordHash = hasher.HashPassword(user2, "Password123!");

            var staff = new User
            {
                Id = staffId,
                UserName = "staff@example.com",
                NormalizedUserName = "STAFF@EXAMPLE.COM",
                Email = "staff@example.com",
                NormalizedEmail = "STAFF@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "Staff",
                LastName = "Member",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            staff.PasswordHash = hasher.HashPassword(staff, "Password123!");

            var admin = new User
            {
                Id = adminId,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Super",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Password123!");

            modelBuilder.Entity<User>().HasData(user1, user2, staff, admin);

            // ---------------------------
            // Koppla användare till roller
            // ---------------------------
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = user1Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = user2Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = staffId, RoleId = staffRoleId },
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId }
            );

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Training", Description = "Physical training and exercise" },
                new Category { Id = 2, Name = "Outdoor Activities", Description = "Activities outdoors" },
                new Category { Id = 3, Name = "Ball Sports", Description = "Football, Tennis, etc." },
                new Category { Id = 4, Name = "Aquatics", Description = "Swimming and water activities" },
                new Category { Id = 5, Name = "Wellness", Description = "Yoga, meditation, etc." }
            );

            // location 
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Gym Hall", Address = "Centralvägen 10, Stockholm", Latitude = 59.3121, Longitude = 18.0674 },
                new Location { Id = 2, Name = "Spinning Room", Address = "Sundbybergsvägen 22, Solna", Latitude = 59.3612, Longitude = 18.0012 },
                new Location { Id = 3, Name = "Climbing Wall", Address = "Rosenlundsgatan 45, Stockholm", Latitude = 59.3129, Longitude = 18.0463 },
                new Location { Id = 4, Name = "Tennis Court", Address = "Lidingövägen 55, Stockholm", Latitude = 59.3478, Longitude = 18.0901 },
                new Location { Id = 5, Name = "Football Field", Address = "Björkhagen 7, Nacka", Latitude = 59.2935, Longitude = 18.1324 },
                new Location { Id = 6, Name = "Swimming Pool", Address = "Stadshagsvägen 12, Stockholm", Latitude = 59.3399, Longitude = 18.0187 },
                new Location { Id = 7, Name = "Spa & Relax", Address = "Drottninggatan 88, Stockholm", Latitude = 59.3334, Longitude = 18.0639 }
            );

            // Zones
             modelBuilder.Entity<Zone>().HasData(
                new Zone { Id = 1, Name = "Yoga & Pilates Studio", IsOutdoor = false, LocaitonId = 1 },
                new Zone { Id = 2, Name = "Spinning Hall", IsOutdoor = false, LocaitonId = 2 },
                new Zone { Id = 3, Name = "Climbing Zone", IsOutdoor = false, LocaitonId = 3 },
                new Zone { Id = 4, Name = "Tennis Zone", IsOutdoor = true, LocaitonId = 4 },
                new Zone { Id = 5, Name = "Football Arena", IsOutdoor = true, LocaitonId = 5 },
                new Zone { Id = 6, Name = "Aquatic Center", IsOutdoor = false, LocaitonId = 6 },
                new Zone { Id = 7, Name = "Spa & Relax Area", IsOutdoor = false, LocaitonId = 7 }
             );

            // Activities (ursprungliga + ny med MaxParticipants=1)
            modelBuilder.Entity<Activity>().HasData(
                new Activity { Id = 1, Name = "Yoga", Description = "Relaxing yoga session", Price = 15, MaxParticipants = 15, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 5, ZoneId = 1, StaffId = staffId },
                new Activity { Id = 2, Name = "Pilates", Description = "Core Pilates class", Price = 15, MaxParticipants = 15, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 5, ZoneId = 1, StaffId = staffId },
                new Activity { Id = 3, Name = "Spinning", Description = "High intensity spinning", Price = 20, MaxParticipants = 20, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 1, ZoneId = 2, StaffId = staffId },
                new Activity { Id = 4, Name = "Football Practice", Description = "Outdoor football training", Price = 10, MaxParticipants = 22, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 3, ZoneId = 5, StaffId = staffId },
                new Activity { Id = 5, Name = "Tennis Practice", Description = "Tennis training session", Price = 12, MaxParticipants = 8, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 3, ZoneId = 4, StaffId = staffId },
                new Activity { Id = 6, Name = "Climbing", Description = "Indoor climbing", Price = 18, MaxParticipants = 10, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 2, ZoneId = 3, StaffId = staffId },
                new Activity { Id = 7, Name = "Swimming Training", Description = "Lap swimming session", Price = 15, MaxParticipants = 12, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 4, ZoneId = 6, StaffId = staffId },
                new Activity { Id = 8, Name = "Water Aerobics", Description = "Fun aquatic exercise", Price = 15, MaxParticipants = 15, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 4, ZoneId = 6, StaffId = staffId },
                new Activity { Id = 9, Name = "Meditation", Description = "Mindfulness meditation", Price = 10, MaxParticipants = 15, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 5, ZoneId = 7, StaffId = staffId },
                new Activity { Id = 10, Name = "Athletics", Description = "Track and field practice", Price = 12, MaxParticipants = 20, IsActive = true, IsPrivate = false, IsAvailable = true, CategoryId = 2, ZoneId = 5, StaffId = staffId },
                new Activity { Id = 11, Name = "One-on-One Coaching", Description = "Individuell coachning", Price = 50, MaxParticipants = 1, IsActive = true, IsPrivate = true, IsAvailable = true, CategoryId = 1, ZoneId = 1, StaffId = staffId }
            );

            // ActivityOccurrences 
            modelBuilder.Entity<ActivityOccurrence>().HasData(
                new ActivityOccurrence { Id = 1, ActivityId = 1, ZoneId = 1, StartTime = new DateTime(2025, 10, 10, 8, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 10, 9, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 2, ActivityId = 2, ZoneId = 1, StartTime = new DateTime(2025, 12, 2, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 2, 11, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 3, ActivityId = 3, ZoneId = 2, StartTime = new DateTime(2025, 12, 3, 18, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 3, 19, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },

                // Fulltest
                new ActivityOccurrence { Id = 11, ActivityId = 3, ZoneId = 2, StartTime = new DateTime(2025, 12, 5, 18, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 5, 19, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },

                // MaxParticipants = 1 
                new ActivityOccurrence { Id = 12, ActivityId = 11, ZoneId = 1, StartTime = new DateTime(2025, 12, 6, 9, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 6, 10, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },

                // Förflutet pass
                new ActivityOccurrence { Id = 13, ActivityId = 1, ZoneId = 1, StartTime = new DateTime(2025, 1, 15, 8, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 1, 15, 9, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },

                // Boundary + överlappning
                new ActivityOccurrence { Id = 14, ActivityId = 2, ZoneId = 1, StartTime = new DateTime(2025, 10, 15, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 15, 11, 0, 0, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 15, ActivityId = 2, ZoneId = 1, StartTime = new DateTime(2025, 10, 15, 10, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 15, 11, 30, 0, DateTimeKind.Utc), DurationMinutes = 60 }
            );

            // Bookings (olika status + canceled + reserved + pending + confirmed)
            var seedBookingTime = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, UserId = user1Id, ActivityOccurrenceId = 11, Status = BookingStatus.Reserved, CreatedAt = seedBookingTime, UpdatedAt = seedBookingTime },
                new Booking { Id = 2, UserId = user2Id, ActivityOccurrenceId = 1, Status = BookingStatus.Canceled, CreatedAt = seedBookingTime, UpdatedAt = seedBookingTime },
                new Booking { Id = 3, UserId = user1Id, ActivityOccurrenceId = 2, Status = BookingStatus.Confirmed, CreatedAt = seedBookingTime, UpdatedAt = seedBookingTime },
                new Booking { Id = 4, UserId = user2Id, ActivityOccurrenceId = 3, Status = BookingStatus.Pending, CreatedAt = seedBookingTime, UpdatedAt = seedBookingTime }
            );
        }
    }
}