using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActiviGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookingSeedExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e9c277f0-a8fb-4817-952f-c8dd450d6a2d"), new Guid("675c59f8-716d-41ee-b015-f10a9466c235") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f6b5f227-0bb1-47c9-a3f8-a57dae5ae559"), new Guid("8ee2d686-9908-46d7-87ee-ad1f9e37e460") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("c6834805-a494-466e-a709-48bed1b0013a"), new Guid("a4b8a110-593b-4486-8398-a234cb12a592") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e9c277f0-a8fb-4817-952f-c8dd450d6a2d"), new Guid("b6aea41e-6d5f-4217-87de-576b346170e9") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c6834805-a494-466e-a709-48bed1b0013a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e9c277f0-a8fb-4817-952f-c8dd450d6a2d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f6b5f227-0bb1-47c9-a3f8-a57dae5ae559"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("675c59f8-716d-41ee-b015-f10a9466c235"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8ee2d686-9908-46d7-87ee-ad1f9e37e460"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a4b8a110-593b-4486-8398-a234cb12a592"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b6aea41e-6d5f-4217-87de-576b346170e9"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Activities",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6a9042c6-f66e-415e-b8b2-a9602c551ed5"), null, "User", "USER" },
                    { new Guid("9819fa13-09e3-4150-88f5-c3020b76c7bc"), null, "Staff", "STAFF" },
                    { new Guid("b8af1b20-7957-4648-a802-be366856ce20"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("381978e8-de89-4656-a430-1943204f8836"), 0, "7a1b53f0-16eb-4af2-8cd5-a5b73c9e7abc", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user1@example.com", true, "User", true, "One", false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKbaVtpMVd8sTuPyCGHI8lVU7Rmm7ufRp7ftH0St26j/YCAxlo8hleIxo5S0CEVTYQ==", null, false, null, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user1@example.com" },
                    { new Guid("6463453a-4df0-4622-97a7-daff920f84d3"), 0, "c13a42e8-59ea-4fed-bcde-2665a997419c", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user2@example.com", true, "User", true, "Two", false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEHzEQMVvebRk78o37e4U4DX66DgflJcUJYQdPvlBhPtzB8BCfcoBCb3cksfCu1rLxw==", null, false, null, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user2@example.com" },
                    { new Guid("a18a9e76-1b38-42e4-b692-0b751069ea40"), 0, "40bc3218-f678-442f-8df2-9239be1dfe44", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@example.com", true, "Admin", true, "Super", false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAELfxR3ElUUzoNEnvlnxVWKgiTKGhUaH4S5tYmd/V94bDqSH/sEyyWe3iZYr2updoYQ==", null, false, null, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@example.com" },
                    { new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), 0, "13bd6483-d68c-42fa-a043-d8e5580e2eb8", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "staff@example.com", true, "Staff", true, "Member", false, null, "STAFF@EXAMPLE.COM", "STAFF@EXAMPLE.COM", "AQAAAAIAAYagAAAAEDGa0VLvL1MAL4thC4c1JuK5L2Zy4cnBx7oqnKb1jN3oX2Hf10eLuHcJeTg9uWvO1w==", null, false, null, false, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "staff@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physical training and exercise", "Training", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Activities outdoors", "Outdoor Activities", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Football, Tennis, etc.", "Ball Sports", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Swimming and water activities", "Aquatics", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yoga, meditation, etc.", "Wellness", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "Address", "CreatedAt", "InOut", "Latitude", "Longitude", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Main Facility - Zone A", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 59.329999999999998, 18.059999999999999, "Gym Hall", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Main Facility - Zone B", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 59.329999999999998, 18.07, "Spinning Room", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Main Facility - Zone C", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 59.329999999999998, 18.079999999999998, "Climbing Wall", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Main Facility - Zone D", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 59.340000000000003, 18.050000000000001, "Tennis Court", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Main Facility - Zone E", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 59.340000000000003, 18.059999999999999, "Football Field", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Main Facility - Zone F", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 59.350000000000001, 18.039999999999999, "Swimming Pool", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Main Facility - Zone G", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 59.350000000000001, 18.050000000000001, "Spa & Relax", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "IsAvailable", "IsPrivate", "MaxParticipants", "Name", "Price", "StaffId", "UpdatedAt", "ZoneId" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Relaxing yoga session", true, true, false, 15, "Yoga", 15m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Core Pilates class", true, true, false, 15, "Pilates", 15m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "High intensity spinning", true, true, false, 20, "Spinning", 20m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Outdoor football training", true, true, false, 22, "Football Practice", 10m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 5, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tennis training session", true, true, false, 8, "Tennis Practice", 12m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 6, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Indoor climbing", true, true, false, 10, "Climbing", 18m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 7, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lap swimming session", true, true, false, 12, "Swimming Training", 15m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 8, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fun aquatic exercise", true, true, false, 15, "Water Aerobics", 15m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 9, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mindfulness meditation", true, true, false, 15, "Meditation", 10m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 10, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Track and field practice", true, true, false, 20, "Athletics", 12m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 11, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Individuell coachning", true, true, true, 1, "One-on-One Coaching", 50m, new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("6a9042c6-f66e-415e-b8b2-a9602c551ed5"), new Guid("381978e8-de89-4656-a430-1943204f8836") },
                    { new Guid("6a9042c6-f66e-415e-b8b2-a9602c551ed5"), new Guid("6463453a-4df0-4622-97a7-daff920f84d3") },
                    { new Guid("b8af1b20-7957-4648-a802-be366856ce20"), new Guid("a18a9e76-1b38-42e4-b692-0b751069ea40") },
                    { new Guid("9819fa13-09e3-4150-88f5-c3020b76c7bc"), new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb") }
                });

            migrationBuilder.InsertData(
                table: "ActivityOccurences",
                columns: new[] { "Id", "ActivityId", "CreatedAt", "DurationMinutes", "EndTime", "StartTime", "UpdatedAt", "ZoneId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 10, 10, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 10, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 2, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 2, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 3, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 3, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 11, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 5, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 5, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 12, 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 6, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 6, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 13, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 14, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 10, 15, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 15, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 15, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 10, 15, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 15, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "ActivityOccurenceId", "CreatedAt", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 11, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("381978e8-de89-4656-a430-1943204f8836") },
                    { 2, 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("6463453a-4df0-4622-97a7-daff920f84d3") },
                    { 3, 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("381978e8-de89-4656-a430-1943204f8836") },
                    { 4, 3, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("6463453a-4df0-4622-97a7-daff920f84d3") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_ActivityOccurenceId",
                table: "Bookings",
                columns: new[] { "UserId", "ActivityOccurenceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId_ActivityOccurenceId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6a9042c6-f66e-415e-b8b2-a9602c551ed5"), new Guid("381978e8-de89-4656-a430-1943204f8836") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6a9042c6-f66e-415e-b8b2-a9602c551ed5"), new Guid("6463453a-4df0-4622-97a7-daff920f84d3") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b8af1b20-7957-4648-a802-be366856ce20"), new Guid("a18a9e76-1b38-42e4-b692-0b751069ea40") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("9819fa13-09e3-4150-88f5-c3020b76c7bc"), new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb") });

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ActivityOccurences",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a9042c6-f66e-415e-b8b2-a9602c551ed5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9819fa13-09e3-4150-88f5-c3020b76c7bc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b8af1b20-7957-4648-a802-be366856ce20"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("381978e8-de89-4656-a430-1943204f8836"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("6463453a-4df0-4622-97a7-daff920f84d3"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18a9e76-1b38-42e4-b692-0b751069ea40"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b06d419b-5d68-47a4-b4b4-56c95de952cb"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Activities",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("c6834805-a494-466e-a709-48bed1b0013a"), null, "Admin", "ADMIN" },
                    { new Guid("e9c277f0-a8fb-4817-952f-c8dd450d6a2d"), null, "User", "USER" },
                    { new Guid("f6b5f227-0bb1-47c9-a3f8-a57dae5ae559"), null, "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("675c59f8-716d-41ee-b015-f10a9466c235"), 0, "9758ae2f-ca91-48d8-8fbb-bb440eeab601", new DateTime(2025, 10, 1, 12, 2, 4, 286, DateTimeKind.Utc).AddTicks(2781), "user2@example.com", true, "User", true, "Two", false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJrujpyOav9RkvaTBex6QNNqpbm5iIokPit34dr3gVAEQajHEHWyQKOvhV6f//xwwg==", null, false, null, false, new DateTime(2025, 10, 1, 12, 2, 4, 286, DateTimeKind.Utc).AddTicks(2787), "user2@example.com" },
                    { new Guid("8ee2d686-9908-46d7-87ee-ad1f9e37e460"), 0, "24e25f5d-805c-40b0-86d4-c603ba84ddb2", new DateTime(2025, 10, 1, 12, 2, 4, 365, DateTimeKind.Utc).AddTicks(625), "staff@example.com", true, "Staff", true, "Member", false, null, "STAFF@EXAMPLE.COM", "STAFF@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJIm6cySnYBskGs6eDypQkRYW1T1soeXm6YYA5UQUTbzUn3NvynWSXj0RnffboNQVQ==", null, false, null, false, new DateTime(2025, 10, 1, 12, 2, 4, 365, DateTimeKind.Utc).AddTicks(631), "staff@example.com" },
                    { new Guid("a4b8a110-593b-4486-8398-a234cb12a592"), 0, "9c719552-1c3e-4bb4-b268-5f923b3c05cb", new DateTime(2025, 10, 1, 12, 2, 4, 441, DateTimeKind.Utc).AddTicks(4423), "admin@example.com", true, "Admin", true, "Super", false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEDB/VM9CrNKhz/4ks6bYYKcE9IM4u7IVNXBFcSvb8uJr2OcVUS70l5sS5e5rLY5hLg==", null, false, null, false, new DateTime(2025, 10, 1, 12, 2, 4, 441, DateTimeKind.Utc).AddTicks(4429), "admin@example.com" },
                    { new Guid("b6aea41e-6d5f-4217-87de-576b346170e9"), 0, "12bced0c-9a67-4789-946a-3463ff7ccd9b", new DateTime(2025, 10, 1, 12, 2, 4, 210, DateTimeKind.Utc).AddTicks(9410), "user1@example.com", true, "User", true, "One", false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIYfwMw4H+04A/cjipWa+WMSm2npSDaJ5XsZsSCZMOrGWtN8NEflODxOBg+ag0DHUw==", null, false, null, false, new DateTime(2025, 10, 1, 12, 2, 4, 210, DateTimeKind.Utc).AddTicks(9412), "user1@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("e9c277f0-a8fb-4817-952f-c8dd450d6a2d"), new Guid("675c59f8-716d-41ee-b015-f10a9466c235") },
                    { new Guid("f6b5f227-0bb1-47c9-a3f8-a57dae5ae559"), new Guid("8ee2d686-9908-46d7-87ee-ad1f9e37e460") },
                    { new Guid("c6834805-a494-466e-a709-48bed1b0013a"), new Guid("a4b8a110-593b-4486-8398-a234cb12a592") },
                    { new Guid("e9c277f0-a8fb-4817-952f-c8dd450d6a2d"), new Guid("b6aea41e-6d5f-4217-87de-576b346170e9") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");
        }
    }
}
