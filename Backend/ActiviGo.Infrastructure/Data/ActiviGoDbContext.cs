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

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Activity>()
                .Property(a => a.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.UserId, b.ActivityOccurrenceId })
                .IsUnique();

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Zone)
                .WithMany(z => z.Activities)
                .HasForeignKey(a => a.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Staff)
                .WithMany(u => u.StaffActivities)
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ActivityOccurrence>()
                .HasOne(o => o.Activity)
                .WithMany(a => a.Occurrences)
                .HasForeignKey(o => o.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ActivityOccurrence)
                .WithMany(o => o.Bookings)
                .HasForeignKey(b => b.ActivityOccurrenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Zone>()
                .HasMany(z => z.Activities)
                .WithOne(a => a.Zone)
                .HasForeignKey(a => a.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Zones)
                .WithOne(z => z.Location)
                .HasForeignKey(z => z.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityOccurrence>()
                .HasOne(o => o.Zone)
                .WithMany(z => z.ActivityOccurrences)
                .HasForeignKey(o => o.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---- Indexes for performance ----
            modelBuilder.Entity<Activity>()
                .HasIndex(a => a.StaffId);
            modelBuilder.Entity<Activity>()
                .HasIndex(a => new { a.ZoneId, a.IsAvailable });
            modelBuilder.Entity<Activity>()
                .HasIndex(a => a.IsAvailable);
            modelBuilder.Entity<ActivityOccurrence>()
                .HasIndex(o => new { o.ActivityId, o.StartTime });
            modelBuilder.Entity<ActivityOccurrence>()
                .HasIndex(o => o.ZoneId);
            modelBuilder.Entity<ActivityOccurrence>()
                .HasIndex(o => o.IsActive);
            modelBuilder.Entity<Zone>()
                .HasIndex(z => new { z.LocationId, z.Name })
                .IsUnique();
            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.ActivityOccurrenceId, b.Status });

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
            var staff2Id = Guid.NewGuid();
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
                FirstName = "Anna",
                LastName = "Andersson",
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
                FirstName = "Bertil",
                LastName = "Berg",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            user2.PasswordHash = hasher.HashPassword(user2, "Password123!");

            var staff = new User
            {
                Id = staffId,
                UserName = "sara@activigo.se",
                NormalizedUserName = "SARA@ACTIVIGO.SE",
                Email = "sara@activigo.se",
                NormalizedEmail = "SARA@ACTIVIGO.SE",
                EmailConfirmed = true,
                FirstName = "Sara",
                LastName = "Sund",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            staff.PasswordHash = hasher.HashPassword(staff, "Password123!");

            var staff2 = new User
            {
                Id = staff2Id,
                UserName = "simon@A.com",
                NormalizedUserName = "SIMON@ACTIVIGO.SE",
                Email = "simon@activigo.se",
                NormalizedEmail = "SIMON@ACTIVIGO.SE",
                EmailConfirmed = true,
                FirstName = "Simon",
                LastName = "Strand",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            staff2.PasswordHash = hasher.HashPassword(staff2, "Password123!");

            var admin = new User
            {
                Id = adminId,
                UserName = "admin@activigo.se",
                NormalizedUserName = "ADMIN@ACTIVIGO.SE",
                Email = "admin@activigo.se",
                NormalizedEmail = "ADMIN@ACTIVIGO.SE",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Super",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Password123!");

            modelBuilder.Entity<User>().HasData(user1, user2, staff, staff2, admin);

            // ---------------------------
            // Koppla användare till roller
            // ---------------------------
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = user1Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = user2Id, RoleId = userRoleId },
                new IdentityUserRole<Guid> { UserId = staffId, RoleId = staffRoleId },
                new IdentityUserRole<Guid> { UserId = staff2Id, RoleId = staffRoleId },
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId }
            );

            // ---------------------------
            // Categories
            // ---------------------------
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Träning", Description = "Fysisk träning och kondition" },
                new Category { Id = 2, Name = "Utomhusaktiviteter", Description = "Aktiviteter utomhus" },
                new Category { Id = 3, Name = "Bollsport", Description = "Fotboll, Tennis m.fl." },
                new Category { Id = 4, Name = "Vattenaktiviteter", Description = "Simning och vatten" },
                new Category { Id = 5, Name = "Wellness", Description = "Yoga, meditation, avslappning" }
            );

            // ---------------------------
            // Locations
            // ---------------------------
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Huvudhall", Address = "Centralvägen 10, Stockholm", Latitude = 59.3121, Longitude = 18.0674 },
                new Location { Id = 2, Name = "Spinninganläggningen", Address = "Sundbybergsvägen 22, Solna", Latitude = 59.3612, Longitude = 18.0012 },
                new Location { Id = 3, Name = "Klätterhallen", Address = "Rosenlundsgatan 45, Stockholm", Latitude = 59.3129, Longitude = 18.0463 },
                new Location { Id = 4, Name = "Tenniscentret", Address = "Lidingövägen 55, Stockholm", Latitude = 59.3478, Longitude = 18.0901 },
                new Location { Id = 5, Name = "Fotbollsanläggningen", Address = "Björkhagen 7, Nacka", Latitude = 59.2935, Longitude = 18.1324 },
                new Location { Id = 6, Name = "Simhallen", Address = "Stadshagsvägen 12, Stockholm", Latitude = 59.3399, Longitude = 18.0187 },
                new Location { Id = 7, Name = "Spa & Relax", Address = "Drottninggatan 88, Stockholm", Latitude = 59.3334, Longitude = 18.0639 },
                new Location { Id = 8, Name = "Träningscenter Solsidan", Address = "Solsidans Allé 1, Stockholm", Latitude = 59.2801, Longitude = 18.2201 }
            );

            // ---------------------------
            // Zones
            // ---------------------------
            modelBuilder.Entity<Zone>().HasData(
                new Zone { Id = 1, Name = "Yoga & Pilates Sal", IsOutdoor = false, LocationId = 1 },
                new Zone { Id = 2, Name = "Spinning Sal", IsOutdoor = false, LocationId = 2 },
                new Zone { Id = 3, Name = "Klättervägg", IsOutdoor = false, LocationId = 3 },
                new Zone { Id = 4, Name = "Tennisbana Utomhus", IsOutdoor = true, LocationId = 4 },
                new Zone { Id = 5, Name = "Fotbollsplan Huvud", IsOutdoor = true, LocationId = 5 },
                new Zone { Id = 6, Name = "25m Bassäng", IsOutdoor = false, LocationId = 6 },
                new Zone { Id = 7, Name = "Relaxavdelning", IsOutdoor = false, LocationId = 7 },
                new Zone { Id = 8, Name = "Stora Gymmet", IsOutdoor = false, LocationId = 8 },
                new Zone { Id = 9, Name = "Lilla Gymmet", IsOutdoor = false, LocationId = 8 },
                new Zone { Id = 10, Name = "Utegym", IsOutdoor = true, LocationId = 8 },
                new Zone { Id = 11, Name = "Tennisbana 1", IsOutdoor = true, LocationId = 8 },
                new Zone { Id = 12, Name = "Tennisbana 2", IsOutdoor = true, LocationId = 8 },
                new Zone { Id = 13, Name = "Multifunktionssal", IsOutdoor = false, LocationId = 8 },
                new Zone { Id = 14, Name = "Fotbollsplan A", IsOutdoor = true, LocationId = 5 },
                new Zone { Id = 15, Name = "Fotbollsplan B", IsOutdoor = true, LocationId = 5 },
                new Zone { Id = 16, Name = "Fotbollsplan C", IsOutdoor = true, LocationId = 5 }
            );

            // ---------------------------
            // Activities
            // ---------------------------
            modelBuilder.Entity<Activity>().HasData(
                new Activity { Id = 1, Name = "Yoga Grund", Description = "Lugn yogaklass för rörlighet och fokus", Price = 150, MaxParticipants = 15, IsPrivate = false, IsAvailable = true, CategoryId = 5, ZoneId = 1, StaffId = staffId, DurationMinutes = 60 },
                new Activity { Id = 2, Name = "Pilates Core", Description = "Stabilitet och bålstyrka", Price = 150, MaxParticipants = 15, IsPrivate = false, IsAvailable = true, CategoryId = 5, ZoneId = 1, StaffId = staff2Id, DurationMinutes = 55 },
                new Activity { Id = 3, Name = "Spinning Intervall", Description = "Högintensiv cykelträning", Price = 180, MaxParticipants = 20, IsPrivate = false, IsAvailable = true, CategoryId = 1, ZoneId = 2, StaffId = staff2Id, DurationMinutes = 45 },
                new Activity { Id = 4, Name = "Fotbollsträning", Description = "Utomhusträning med bollteknik", Price = 120, MaxParticipants = 22, IsPrivate = false, IsAvailable = true, CategoryId = 3, ZoneId = 5, StaffId = staffId, DurationMinutes = 90 },
                new Activity { Id = 5, Name = "Klättring Introduktion", Description = "Grundläggande säkerhet och teknik", Price = 200, MaxParticipants = 10, IsPrivate = false, IsAvailable = true, CategoryId = 2, ZoneId = 3, StaffId = staff2Id, DurationMinutes = 75 },
                new Activity { Id = 6, Name = "Simteknik", Description = "Teknikpass för bättre effektivitet i vattnet", Price = 160, MaxParticipants = 12, IsPrivate = false, IsAvailable = true, CategoryId = 4, ZoneId = 6, StaffId = staffId, DurationMinutes = 60 },
                new Activity { Id = 7, Name = "Relax & Meditation", Description = "Avslappning och mental återhämtning", Price = 100, MaxParticipants = 12, IsPrivate = false, IsAvailable = true, CategoryId = 5, ZoneId = 7, StaffId = staffId, DurationMinutes = 45 },
                new Activity { Id = 8, Name = "Styrketräning Bas", Description = "Instruktörsletta baslyft och teknik", Price = 170, MaxParticipants = 16, IsPrivate = false, IsAvailable = true, CategoryId = 1, ZoneId = 8, StaffId = staffId, DurationMinutes = 60 },
                new Activity { Id = 9, Name = "HIIT Express", Description = "Kort och intensivt pass", Price = 140, MaxParticipants = 14, IsPrivate = false, IsAvailable = true, CategoryId = 1, ZoneId = 9, StaffId = staff2Id, DurationMinutes = 35 },
                new Activity { Id = 10, Name = "Utegym Cirkel", Description = "Cirkelträning utomhus", Price = 30, MaxParticipants = 20, IsPrivate = false, IsAvailable = true, CategoryId = 2, ZoneId = 10, StaffId = staff2Id, DurationMinutes = 50 },
                new Activity { Id = 11, Name = "Tennis Singel", Description = "Boka bana för singelspel", Price = 120, MaxParticipants = 2, IsPrivate = true, IsAvailable = true, CategoryId = 3, ZoneId = 11, StaffId = null, DurationMinutes = 60 },
                new Activity { Id = 12, Name = "Tennis Dubbel", Description = "Boka bana för dubbelspel", Price = 160, MaxParticipants = 4, IsPrivate = true, IsAvailable = true, CategoryId = 3, ZoneId = 12, StaffId = null, DurationMinutes = 60 },
                new Activity { Id = 13, Name = "Öppet Gym", Description = "Öppen tillgång till gymmet", Price = 90, MaxParticipants = 40, IsPrivate = false, IsAvailable = true, CategoryId = 1, ZoneId = 8, StaffId = null, DurationMinutes = 120 },
                new Activity { Id = 14, Name = "Öppet Utegym", Description = "Självservice utomhusträning", Price = 50, MaxParticipants = 30, IsPrivate = false, IsAvailable = true, CategoryId = 2, ZoneId = 10, StaffId = null, DurationMinutes = 120 },
                new Activity { Id = 15, Name = "Multifunktion Flex", Description = "Allsidigt pass i multifunktionssal", Price = 150, MaxParticipants = 18, IsPrivate = false, IsAvailable = true, CategoryId = 1, ZoneId = 13, StaffId = staff2Id, DurationMinutes = 55 },
                new Activity { Id = 16, Name = "Fotboll Öppen Träning", Description = "Spontan fotboll – drop-in", Price = 50, MaxParticipants = 28, IsPrivate = false, IsAvailable = true, CategoryId = 3, ZoneId = 14, StaffId = null, DurationMinutes = 90 },
                new Activity { Id = 17, Name = "Bokning Fotbollsplan A", Description = "Självservice bokning av plan", Price = 200, MaxParticipants = 22, IsPrivate = true, IsAvailable = true, CategoryId = 3, ZoneId = 14, StaffId = null, DurationMinutes = 120 }
            );

            // ---------------------------
            // Activity Occurrences
            // ---------------------------
            modelBuilder.Entity<ActivityOccurrence>().HasData(
                new ActivityOccurrence { Id = 1, ActivityId = 1, ZoneId = 1, StartTime = new DateTime(2025, 10, 10, 08, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 10, 09, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 2, ActivityId = 2, ZoneId = 1, StartTime = new DateTime(2025, 12, 2, 10, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 2, 11, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 3, ActivityId = 3, ZoneId = 2, StartTime = new DateTime(2025, 12, 3, 18, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 3, 19, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 11, ActivityId = 3, ZoneId = 2, StartTime = new DateTime(2025, 12, 5, 18, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 5, 19, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 12, ActivityId = 4, ZoneId = 5, StartTime = new DateTime(2025, 12, 6, 09, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 6, 10, 30, 00, DateTimeKind.Utc), DurationMinutes = 90 },
                new ActivityOccurrence { Id = 13, ActivityId = 1, ZoneId = 1, StartTime = new DateTime(2024, 12, 15, 08, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2024, 12, 15, 09, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 14, ActivityId = 2, ZoneId = 1, StartTime = new DateTime(2025, 10, 15, 10, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 15, 11, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 15, ActivityId = 2, ZoneId = 1, StartTime = new DateTime(2025, 10, 15, 10, 30, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 15, 11, 30, 00, DateTimeKind.Utc), DurationMinutes = 60 },

                new ActivityOccurrence { Id = 16, ActivityId = 8, ZoneId = 8, StartTime = new DateTime(2025, 11, 1, 07, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 1, 08, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 17, ActivityId = 9, ZoneId = 9, StartTime = new DateTime(2025, 11, 1, 12, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 1, 12, 35, 00, DateTimeKind.Utc), DurationMinutes = 35 },
                new ActivityOccurrence { Id = 18, ActivityId = 10, ZoneId = 10, StartTime = new DateTime(2025, 11, 2, 16, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 2, 16, 50, 00, DateTimeKind.Utc), DurationMinutes = 50 },
                new ActivityOccurrence { Id = 19, ActivityId = 11, ZoneId = 11, StartTime = new DateTime(2025, 11, 3, 14, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 3, 15, 00, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 20, ActivityId = 12, ZoneId = 12, StartTime = new DateTime(2025, 11, 3, 15, 30, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 3, 16, 30, 00, DateTimeKind.Utc), DurationMinutes = 60 },
                new ActivityOccurrence { Id = 21, ActivityId = 13, ZoneId = 8, StartTime = new DateTime(2025, 11, 4, 06, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 4, 08, 00, 00, DateTimeKind.Utc), DurationMinutes = 120 },
                new ActivityOccurrence { Id = 22, ActivityId = 14, ZoneId = 10, StartTime = new DateTime(2025, 11, 4, 09, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 4, 11, 00, 00, DateTimeKind.Utc), DurationMinutes = 120 },
                new ActivityOccurrence { Id = 23, ActivityId = 15, ZoneId = 13, StartTime = new DateTime(2025, 11, 5, 18, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 5, 18, 55, 00, DateTimeKind.Utc), DurationMinutes = 55 },
                new ActivityOccurrence { Id = 24, ActivityId = 16, ZoneId = 14, StartTime = new DateTime(2025, 11, 6, 17, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 6, 18, 30, 00, DateTimeKind.Utc), DurationMinutes = 90 },
                new ActivityOccurrence { Id = 25, ActivityId = 17, ZoneId = 14, StartTime = new DateTime(2025, 11, 7, 10, 00, 00, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 7, 12, 00, 00, DateTimeKind.Utc), DurationMinutes = 120 }
            );

            // ---------------------------
            // Bookings
            // ---------------------------
            var bookingSeedTime = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, UserId = user1Id, ActivityOccurrenceId = 11, Status = BookingStatus.Reserved, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 2, UserId = user2Id, ActivityOccurrenceId = 1, Status = BookingStatus.Canceled, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 3, UserId = user1Id, ActivityOccurrenceId = 2, Status = BookingStatus.Confirmed, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 4, UserId = user2Id, ActivityOccurrenceId = 3, Status = BookingStatus.Pending, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 5, UserId = user1Id, ActivityOccurrenceId = 16, Status = BookingStatus.Reserved, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 6, UserId = user2Id, ActivityOccurrenceId = 19, Status = BookingStatus.Reserved, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 7, UserId = user1Id, ActivityOccurrenceId = 20, Status = BookingStatus.Reserved, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime },
                new Booking { Id = 8, UserId = user2Id, ActivityOccurrenceId = 23, Status = BookingStatus.Pending, CreatedAt = bookingSeedTime, UpdatedAt = bookingSeedTime }
            );
        }
    }
}