using ActiviGo.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.Infrastructure.Data
{
    public class ActiviGoDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ActiviGoDbContext(DbContextOptions<ActiviGoDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityOccurence> ActivityOccurences { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

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
            // --- Roles ---
            var userRoleId = Guid.NewGuid();
            var staffRoleId = Guid.NewGuid();
            var adminRoleId = Guid.NewGuid();

            modelBuilder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = userRoleId, Name = "User", NormalizedName = "USER" },
                new IdentityRole<Guid> { Id = staffRoleId, Name = "Staff", NormalizedName = "STAFF" },
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" }
            );

            // --- Users ---
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
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hasher.HashPassword(null, "Password123!")
            };

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
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hasher.HashPassword(null, "Password123!")
            };

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
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hasher.HashPassword(null, "Password123!")
            };

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
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hasher.HashPassword(null, "Password123!")
            };

            modelBuilder.Entity<User>().HasData(user1, user2, staff, admin);

            // --- UserRoles ---
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = user1Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = user2Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = staffId, RoleId = staffRoleId },
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId }
            );

            //    // --- Categories ---
            //    modelBuilder.Entity<Category>().HasData(
            //        new Category
            //        {
            //            Id = 1,
            //            Name = "Training",
            //            Description = "Physical training and exercise"
            //        },
            //        new Category
            //        {
            //            Id = 2,
            //            Name = "Outdoor Activities",
            //            Description = "Activities outdoors"
            //        },
            //        new Category
            //        {
            //            Id = 3,
            //            Name = "Ball Sports",
            //            Description = "Football, Tennis, etc."
            //        },
            //        new Category
            //        {
            //            Id = 4,
            //            Name = "Aquatics",
            //            Description = "Swimming and water activities"
            //        },
            //        new Category
            //        {
            //            Id = 5,
            //            Name = "Wellness",
            //            Description = "Yoga, meditation, etc."
            //        }
            //    );

            //    // --- Zones ---
            //    modelBuilder.Entity<Zone>().HasData(
            //        new Zone
            //        {
            //            Id = 1,
            //            Name = "Gym Hall",
            //            Address = "Main Facility - Zone A",
            //            Latitude = 59.33,
            //            Longitude = 18.06,
            //            InOut = ZoneType.Indoor
            //        },
            //        new Zone
            //        {
            //            Id = 2,
            //            Name = "Spinning Room",
            //            Address = "Main Facility - Zone B",
            //            Latitude = 59.33,
            //            Longitude = 18.07,
            //            InOut = ZoneType.Indoor
            //        },
            //        new Zone
            //        {
            //            Id = 3,
            //            Name = "Climbing Wall",
            //            Address = "Main Facility - Zone C",
            //            Latitude = 59.33,
            //            Longitude = 18.08,
            //            InOut = ZoneType.Indoor
            //        },
            //        new Zone
            //        {
            //            Id = 4,
            //            Name = "Tennis Court",
            //            Address = "Main Facility - Zone D",
            //            Latitude = 59.34,
            //            Longitude = 18.05,
            //            InOut = ZoneType.Outdoor
            //        },
            //        new Zone
            //        {
            //            Id = 5,
            //            Name = "Football Field",
            //            Address = "Main Facility - Zone E",
            //            Latitude = 59.34,
            //            Longitude = 18.06,
            //            InOut = ZoneType.Outdoor
            //        },
            //        new Zone
            //        {
            //            Id = 6,
            //            Name = "Swimming Pool",
            //            Address = "Main Facility - Zone F",
            //            Latitude = 59.35,
            //            Longitude = 18.04,
            //            InOut = ZoneType.Indoor
            //        },
            //        new Zone
            //        {
            //            Id = 7,
            //            Name = "Spa & Relax",
            //            Address = "Main Facility - Zone G",
            //            Latitude = 59.35,
            //            Longitude = 18.05,
            //            InOut = ZoneType.Indoor
            //        }
            //    );

            //    // --- Activities ---
            //    modelBuilder.Entity<Activity>().HasData(
            //        new Activity
            //        {
            //            Id = 1,
            //            Name = "Yoga",
            //            Description = "Relaxing yoga session",
            //            Price = 15,
            //            MaxParticipants = 15,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 5,
            //            ZoneId = 1,
            //            StaffId = 4
            //        },
            //        new Activity
            //        {
            //            Id = 2,
            //            Name = "Pilates",
            //            Description = "Core Pilates class",
            //            Price = 15,
            //            MaxParticipants = 15,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 5,
            //            ZoneId = 1,
            //            StaffId = 4
            //        },

            //        new Activity
            //        {
            //            Id = 3,
            //            Name = "Spinning",
            //            Description = "High intensity spinning",
            //            Price = 20,
            //            MaxParticipants = 20,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 1,
            //            ZoneId = 2,
            //            StaffId = 5
            //        },
            //        new Activity
            //        {
            //            Id = 4,
            //            Name = "Football Practice",
            //            Description = "Outdoor football training",
            //            Price = 10,
            //            MaxParticipants = 22,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 3,
            //            ZoneId = 5,
            //            StaffId = 5
            //        },
            //        new Activity
            //        {
            //            Id = 5,
            //            Name = "Tennis Practice",
            //            Description = "Tennis training session",
            //            Price = 12,
            //            MaxParticipants = 8,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 3,
            //            ZoneId = 4,
            //            StaffId = 5
            //        },
            //        new Activity
            //        {
            //            Id = 6,
            //            Name = "Climbing",
            //            Description = "Indoor climbing",
            //            Price = 18,
            //            MaxParticipants = 10,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 2,
            //            ZoneId = 3,
            //            StaffId = 4
            //        },
            //        new Activity
            //        {
            //            Id = 7,
            //            Name = "Swimming Training",
            //            Description = "Lap swimming session",
            //            Price = 15,
            //            MaxParticipants = 12,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 4,
            //            ZoneId = 6,
            //            StaffId = 5
            //        },
            //        new Activity
            //        {
            //            Id = 8,
            //            Name = "Water Aerobics",
            //            Description = "Fun aquatic exercise",
            //            Price = 15,
            //            MaxParticipants = 15,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 4,
            //            ZoneId = 6,
            //            StaffId = 4
            //        },
            //        new Activity
            //        {
            //            Id = 9,
            //            Name = "Meditation",
            //            Description = "Mindfulness meditation",
            //            Price = 10,
            //            MaxParticipants = 15,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 5,
            //            ZoneId = 7,
            //            StaffId = 4
            //        },
            //        new Activity
            //        {
            //            Id = 10,
            //            Name = "Athletics",
            //            Description = "Track and field practice",
            //            Price = 12,
            //            MaxParticipants = 20,
            //            IsActive = true,
            //            IsPrivate = false,
            //            IsAvailable = true,
            //            CategoryId = 2,
            //            ZoneId = 5,
            //            StaffId = 5
            //        }
            //    );

            //    // --- ActivityOccurrences ---
            //    modelBuilder.Entity<ActivityOccurence>().HasData(
            //        new ActivityOccurence
            //        {
            //            Id = 1,
            //            ActivityId = 1,
            //            ZoneId = 1,
            //            StartTime = new DateTime(2025, 10, 1, 8, 0, 0),
            //            EndTime = new DateTime(2025, 10, 1, 9, 0, 0),
            //            DurationMinutes = 60
            //        },
            //        new ActivityOccurence
            //        {
            //            Id = 2,
            //            ActivityId = 2,
            //            ZoneId = 1,
            //            StartTime = new DateTime(2025, 10, 2, 10, 0, 0),
            //            EndTime = new DateTime(2025, 10, 2, 11, 0, 0),
            //            DurationMinutes = 60
            //        },
            //        new ActivityOccurence
            //        {
            //            Id = 3,
            //            ActivityId = 3,
            //            ZoneId = 2,
            //            StartTime = new DateTime(2025, 10, 3, 18, 0, 0),
            //            EndTime = new DateTime(2025, 10, 3, 19, 0, 0),
            //            DurationMinutes = 60
            //        }
            //    );
            //}
        }


    }
}


