using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActiviGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsOutdoor = table.Column<bool>(type: "bit", nullable: false),
                    LocaitonId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_Locations_LocaitonId",
                        column: x => x.LocaitonId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityOccurrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityOccurrences_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityOccurrences_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityOccurrenceId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_ActivityOccurrences_ActivityOccurrenceId",
                        column: x => x.ActivityOccurrenceId,
                        principalTable: "ActivityOccurrences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("39b1edae-9b01-4cff-b151-6f7b4af5dbf6"), null, "Admin", "ADMIN" },
                    { new Guid("90921b8a-db4f-4466-ae25-58fd99468646"), null, "Staff", "STAFF" },
                    { new Guid("ce8e3343-946c-44a6-bf1e-b1ca1cfb4855"), null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("4eaf5b21-8f0b-4bcf-9c8b-6af7d2c933c6"), 0, "5ba9cb82-bdd7-44ff-9130-9ffc5a835f33", new DateTime(2025, 10, 7, 10, 59, 24, 464, DateTimeKind.Utc).AddTicks(2916), "user1@example.com", true, "User", true, "One", false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEDzA1O+T0IMVDI0mtVcVkft+og1aEnXRcDUFHcRIstVsqGbsDpS0cEWJoyk0KUir3Q==", null, false, null, false, new DateTime(2025, 10, 7, 10, 59, 24, 464, DateTimeKind.Utc).AddTicks(2920), "user1@example.com" },
                    { new Guid("a1c5fd4c-ec97-4eb5-9a67-135a3b1807f1"), 0, "11fea9e7-262c-451a-abfb-99509bc17eed", new DateTime(2025, 10, 7, 10, 59, 24, 538, DateTimeKind.Utc).AddTicks(3112), "user2@example.com", true, "User", true, "Two", false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEE6EALum5t/GdbVR0ALKnXmgPJwwxCquiRfjziEk2k/cfP6EffFpcwDIGqc+uzRIJw==", null, false, null, false, new DateTime(2025, 10, 7, 10, 59, 24, 538, DateTimeKind.Utc).AddTicks(3125), "user2@example.com" },
                    { new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), 0, "e4e63781-d2d5-4b05-bfd1-a1cfc07294c1", new DateTime(2025, 10, 7, 10, 59, 24, 637, DateTimeKind.Utc).AddTicks(229), "staff@example.com", true, "Staff", true, "Member", false, null, "STAFF@EXAMPLE.COM", "STAFF@EXAMPLE.COM", "AQAAAAIAAYagAAAAELJSqojiO9kPD4BYwYyBasv7F3GfFQJNa1k53WfW+1PJNVn3wsvaXxDyqMdnDLiC2Q==", null, false, null, false, new DateTime(2025, 10, 7, 10, 59, 24, 637, DateTimeKind.Utc).AddTicks(241), "staff@example.com" },
                    { new Guid("ef2543ac-2372-4d23-99ab-43498d2bdc77"), 0, "a2bb878f-e7f3-43f4-bb3f-4ff76da22dcb", new DateTime(2025, 10, 7, 10, 59, 24, 716, DateTimeKind.Utc).AddTicks(8779), "admin@example.com", true, "Admin", true, "Super", false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIfJCJopyfdvJXqyXTJ5SUWQZnMJActryrGhlZX5XhZUKw4hCSWzUBxNHgjuNL/fww==", null, false, null, false, new DateTime(2025, 10, 7, 10, 59, 24, 716, DateTimeKind.Utc).AddTicks(8786), "admin@example.com" }
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
                table: "Locations",
                columns: new[] { "Id", "Address", "CreatedAt", "Latitude", "Longitude", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Centralvägen 10, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.312100000000001, 18.067399999999999, "Gym Hall", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Sundbybergsvägen 22, Solna", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.361199999999997, 18.001200000000001, "Spinning Room", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Rosenlundsgatan 45, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.312899999999999, 18.046299999999999, "Climbing Wall", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Lidingövägen 55, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.347799999999999, 18.0901, "Tennis Court", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Björkhagen 7, Nacka", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.293500000000002, 18.132400000000001, "Football Field", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Stadshagsvägen 12, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.3399, 18.018699999999999, "Swimming Pool", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Drottninggatan 88, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.333399999999997, 18.0639, "Spa & Relax", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("ce8e3343-946c-44a6-bf1e-b1ca1cfb4855"), new Guid("4eaf5b21-8f0b-4bcf-9c8b-6af7d2c933c6") },
                    { new Guid("ce8e3343-946c-44a6-bf1e-b1ca1cfb4855"), new Guid("a1c5fd4c-ec97-4eb5-9a67-135a3b1807f1") },
                    { new Guid("90921b8a-db4f-4466-ae25-58fd99468646"), new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e") },
                    { new Guid("39b1edae-9b01-4cff-b151-6f7b4af5dbf6"), new Guid("ef2543ac-2372-4d23-99ab-43498d2bdc77") }
                });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "CreatedAt", "IsOutdoor", "LocaitonId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "Yoga & Pilates Studio", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, "Spinning Hall", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, "Climbing Zone", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Tennis Zone", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Football Arena", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6, "Aquatic Center", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 7, "Spa & Relax Area", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "DurationMinutes", "IsActive", "IsAvailable", "IsPrivate", "MaxParticipants", "Name", "Price", "StaffId", "UpdatedAt", "ZoneId" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Relaxing yoga session", 0, true, true, false, 15, "Yoga", 15m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Core Pilates class", 0, true, true, false, 15, "Pilates", 15m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "High intensity spinning", 0, true, true, false, 20, "Spinning", 20m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Outdoor football training", 0, true, true, false, 22, "Football Practice", 10m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 5, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tennis training session", 0, true, true, false, 8, "Tennis Practice", 12m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 6, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Indoor climbing", 0, true, true, false, 10, "Climbing", 18m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 7, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lap swimming session", 0, true, true, false, 12, "Swimming Training", 15m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 8, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fun aquatic exercise", 0, true, true, false, 15, "Water Aerobics", 15m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 9, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mindfulness meditation", 0, true, true, false, 15, "Meditation", 10m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 10, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Track and field practice", 0, true, true, false, 20, "Athletics", 12m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 11, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Individuell coachning", 0, true, true, true, 1, "One-on-One Coaching", 50m, new Guid("cfe2a822-daeb-4bb4-88a9-eed19d24bc3e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "ActivityOccurrences",
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
                columns: new[] { "Id", "ActivityOccurrenceId", "CreatedAt", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 11, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("4eaf5b21-8f0b-4bcf-9c8b-6af7d2c933c6") },
                    { 2, 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a1c5fd4c-ec97-4eb5-9a67-135a3b1807f1") },
                    { 3, 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("4eaf5b21-8f0b-4bcf-9c8b-6af7d2c933c6") },
                    { 4, 3, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a1c5fd4c-ec97-4eb5-9a67-135a3b1807f1") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CategoryId",
                table: "Activities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StaffId",
                table: "Activities",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ZoneId",
                table: "Activities",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityOccurrences_ActivityId",
                table: "ActivityOccurrences",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityOccurrences_ZoneId",
                table: "ActivityOccurrences",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ActivityOccurrenceId",
                table: "Bookings",
                column: "ActivityOccurrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_ActivityOccurrenceId",
                table: "Bookings",
                columns: new[] { "UserId", "ActivityOccurrenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_LocaitonId",
                table: "Zones",
                column: "LocaitonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ActivityOccurrences");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
