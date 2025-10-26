using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ActiviGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedModels : Migration
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
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityOccurrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    { new Guid("4e05514c-d583-4c2c-b55b-97a44aaeaf73"), null, "User", "USER" },
                    { new Guid("8e0861fc-c798-41b0-8bfb-e24a7979c4ec"), null, "Admin", "ADMIN" },
                    { new Guid("c4bb39b1-d711-4d39-8e70-7b27dae15bb3"), null, "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("240babc8-287b-49a9-904f-ff81d301eb50"), 0, "66aa839d-b8c4-4c84-aca2-0aeeaad6d7ed", new DateTime(2025, 10, 24, 12, 36, 4, 899, DateTimeKind.Utc).AddTicks(9059), "user1@example.com", true, "Anna", true, "Andersson", false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEqk7M4rEf69t/ngi65PW1NTmRBUwXBIBE18h75r33uyCXYsLBIXQ8oo4/jaLOuJfw==", null, false, null, false, new DateTime(2025, 10, 24, 12, 36, 4, 899, DateTimeKind.Utc).AddTicks(9067), "user1@example.com" },
                    { new Guid("34e150ae-706e-46cb-8043-199f15adc38d"), 0, "b704333e-a731-4853-9ea8-84ecada89d1c", new DateTime(2025, 10, 24, 12, 36, 5, 152, DateTimeKind.Utc).AddTicks(3445), "sara@activigo.se", true, "Sara", true, "Sund", false, null, "SARA@ACTIVIGO.SE", "SARA@ACTIVIGO.SE", "AQAAAAIAAYagAAAAEI+8HrgwoVHaBEqmBMnbUiW0d4FXlY9g5fQ3Z/oX3/nFFHNSkuRPxp4HWk+Jjg1oJw==", null, false, null, false, new DateTime(2025, 10, 24, 12, 36, 5, 152, DateTimeKind.Utc).AddTicks(3454), "sara@activigo.se" },
                    { new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), 0, "80058734-38f2-4857-9350-adfa56e0510d", new DateTime(2025, 10, 24, 12, 36, 5, 305, DateTimeKind.Utc).AddTicks(736), "simon@activigo.se", true, "Simon", true, "Strand", false, null, "SIMON@ACTIVIGO.SE", "SIMON@ACTIVIGO.SE", "AQAAAAIAAYagAAAAEDfspFCZNXYeodwYcjiIU0kABKCP7YuvoC8ehc33UtOc9l/L+lXhHg73SEV+j7KrKQ==", null, false, null, false, new DateTime(2025, 10, 24, 12, 36, 5, 305, DateTimeKind.Utc).AddTicks(742), "simon@A.com" },
                    { new Guid("ba556bf4-37ae-49c1-9c73-3ed329956a79"), 0, "93c133bd-380a-4589-bcec-a259ffcadd7e", new DateTime(2025, 10, 24, 12, 36, 5, 29, DateTimeKind.Utc).AddTicks(3572), "user2@example.com", true, "Bertil", true, "Berg", false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGJdARdxyeL0h6AUfxRNCHcT5Fb1gp9ygPJ3viWOZZBXn0xWHxCWwv+amXuojNoRDQ==", null, false, null, false, new DateTime(2025, 10, 24, 12, 36, 5, 29, DateTimeKind.Utc).AddTicks(3579), "user2@example.com" },
                    { new Guid("f3d6f133-7c9a-47a7-92e0-1f1ce7592e68"), 0, "4bd2a149-0fca-4593-8934-2d339f6e0f80", new DateTime(2025, 10, 24, 12, 36, 5, 443, DateTimeKind.Utc).AddTicks(1952), "admin@activigo.se", true, "Admin", true, "Super", false, null, "ADMIN@ACTIVIGO.SE", "ADMIN@ACTIVIGO.SE", "AQAAAAIAAYagAAAAEFsHNB5mp4fE7n4xbcb5yJsq1siwBrAfx+wWp2BJJp9a1DVnqRxEXdq/3s8Z47yoQA==", null, false, null, false, new DateTime(2025, 10, 24, 12, 36, 5, 443, DateTimeKind.Utc).AddTicks(1958), "admin@activigo.se" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fysisk träning och kondition", "Träning", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktiviteter utomhus", "Utomhusaktiviteter", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fotboll, Tennis m.fl.", "Bollsport", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simning och vatten", "Vattenaktiviteter", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yoga, meditation, avslappning", "Wellness", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "CreatedAt", "Latitude", "Longitude", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Centralvägen 10, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.312100000000001, 18.067399999999999, "Huvudhall", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Sundbybergsvägen 22, Solna", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.361199999999997, 18.001200000000001, "Spinninganläggningen", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Rosenlundsgatan 45, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.312899999999999, 18.046299999999999, "Klätterhallen", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Lidingövägen 55, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.347799999999999, 18.0901, "Tenniscentret", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Björkhagen 7, Nacka", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.293500000000002, 18.132400000000001, "Fotbollsanläggningen", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Stadshagsvägen 12, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.3399, 18.018699999999999, "Simhallen", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Drottninggatan 88, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.333399999999997, 18.0639, "Spa & Relax", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Solsidans Allé 1, Stockholm", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59.280099999999997, 18.220099999999999, "Träningscenter Solsidan", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("4e05514c-d583-4c2c-b55b-97a44aaeaf73"), new Guid("240babc8-287b-49a9-904f-ff81d301eb50") },
                    { new Guid("c4bb39b1-d711-4d39-8e70-7b27dae15bb3"), new Guid("34e150ae-706e-46cb-8043-199f15adc38d") },
                    { new Guid("c4bb39b1-d711-4d39-8e70-7b27dae15bb3"), new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a") },
                    { new Guid("4e05514c-d583-4c2c-b55b-97a44aaeaf73"), new Guid("ba556bf4-37ae-49c1-9c73-3ed329956a79") },
                    { new Guid("8e0861fc-c798-41b0-8bfb-e24a7979c4ec"), new Guid("f3d6f133-7c9a-47a7-92e0-1f1ce7592e68") }
                });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "CreatedAt", "IsOutdoor", "LocationId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "Yoga & Pilates Sal", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, "Spinning Sal", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, "Klättervägg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, "Tennisbana Utomhus", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Fotbollsplan Huvud", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 6, "25m Bassäng", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 7, "Relaxavdelning", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 8, "Stora Gymmet", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 8, "Lilla Gymmet", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "Utegym", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "Tennisbana 1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 8, "Tennisbana 2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 8, "Multifunktionssal", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Fotbollsplan A", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Fotbollsplan B", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 5, "Fotbollsplan C", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "DurationMinutes", "ImageUrl", "IsAvailable", "IsPrivate", "MaxParticipants", "Name", "Price", "StaffId", "UpdatedAt", "ZoneId" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lugn yogaklass för rörlighet och fokus", 60, "", true, false, 15, "Yoga Grund", 150m, new Guid("34e150ae-706e-46cb-8043-199f15adc38d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stabilitet och bålstyrka", 55, "", true, false, 15, "Pilates Core", 150m, new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Högintensiv cykelträning", 45, "", true, false, 20, "Spinning Intervall", 180m, new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 4, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utomhusträning med bollteknik", 90, "", true, false, 22, "Fotbollsträning", 120m, new Guid("34e150ae-706e-46cb-8043-199f15adc38d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 5, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grundläggande säkerhet och teknik", 75, "", true, false, 10, "Klättring Introduktion", 200m, new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 6, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknikpass för bättre effektivitet i vattnet", 60, "", true, false, 12, "Simteknik", 160m, new Guid("34e150ae-706e-46cb-8043-199f15adc38d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 7, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avslappning och mental återhämtning", 45, "", true, false, 12, "Relax & Meditation", 100m, new Guid("34e150ae-706e-46cb-8043-199f15adc38d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 8, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Instruktörsletta baslyft och teknik", 60, "", true, false, 16, "Styrketräning Bas", 170m, new Guid("34e150ae-706e-46cb-8043-199f15adc38d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 9, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kort och intensivt pass", 35, "", true, false, 14, "HIIT Express", 140m, new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 10, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cirkelträning utomhus", 50, "", true, false, 20, "Utegym Cirkel", 30m, new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 11, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boka bana för singelspel", 60, "", true, true, 2, "Tennis Singel", 120m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 12, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boka bana för dubbelspel", 60, "", true, true, 4, "Tennis Dubbel", 160m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 13, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Öppen tillgång till gymmet", 120, "", true, false, 40, "Öppet Gym", 90m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 14, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Självservice utomhusträning", 120, "", true, false, 30, "Öppet Utegym", 50m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 15, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Allsidigt pass i multifunktionssal", 55, "", true, false, 18, "Multifunktion Flex", 150m, new Guid("a673d0a7-7719-45bd-80b3-a0b5055fb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 16, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spontan fotboll – drop-in", 90, "", true, false, 28, "Fotboll Öppen Träning", 50m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 17, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Självservice bokning av plan", 120, "", true, true, 22, "Bokning Fotbollsplan A", 200m, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 }
                });

            migrationBuilder.InsertData(
                table: "ActivityOccurrences",
                columns: new[] { "Id", "ActivityId", "CreatedAt", "DurationMinutes", "EndTime", "IsActive", "StartTime", "UpdatedAt", "ZoneId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 10, 10, 9, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 10, 10, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 2, 11, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 12, 2, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 3, 19, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 12, 3, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 11, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 12, 5, 19, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 12, 5, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 12, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, new DateTime(2025, 12, 6, 10, 30, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 12, 6, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 13, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2024, 12, 15, 9, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2024, 12, 15, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 14, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 10, 15, 11, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 10, 15, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 15, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 10, 15, 11, 30, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 10, 15, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 16, 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 11, 1, 8, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 1, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 17, 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, new DateTime(2025, 11, 1, 12, 35, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 18, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, new DateTime(2025, 11, 2, 16, 50, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 2, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 19, 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 11, 3, 15, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 3, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 20, 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(2025, 11, 3, 16, 30, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 3, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 21, 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120, new DateTime(2025, 11, 4, 8, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 4, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 22, 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120, new DateTime(2025, 11, 4, 11, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 4, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 23, 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, new DateTime(2025, 11, 5, 18, 55, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 5, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 24, 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, new DateTime(2025, 11, 6, 18, 30, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 6, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 },
                    { 25, 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120, new DateTime(2025, 11, 7, 12, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 11, 7, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "ActivityOccurrenceId", "CreatedAt", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 11, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("240babc8-287b-49a9-904f-ff81d301eb50") },
                    { 2, 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ba556bf4-37ae-49c1-9c73-3ed329956a79") },
                    { 3, 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("240babc8-287b-49a9-904f-ff81d301eb50") },
                    { 4, 3, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ba556bf4-37ae-49c1-9c73-3ed329956a79") },
                    { 5, 16, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("240babc8-287b-49a9-904f-ff81d301eb50") },
                    { 6, 19, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ba556bf4-37ae-49c1-9c73-3ed329956a79") },
                    { 7, 20, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("240babc8-287b-49a9-904f-ff81d301eb50") },
                    { 8, 23, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("ba556bf4-37ae-49c1-9c73-3ed329956a79") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CategoryId",
                table: "Activities",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IsAvailable",
                table: "Activities",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StaffId",
                table: "Activities",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ZoneId_IsAvailable",
                table: "Activities",
                columns: new[] { "ZoneId", "IsAvailable" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityOccurrences_ActivityId_StartTime",
                table: "ActivityOccurrences",
                columns: new[] { "ActivityId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityOccurrences_IsActive",
                table: "ActivityOccurrences",
                column: "IsActive");

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
                name: "IX_Bookings_ActivityOccurrenceId_Status",
                table: "Bookings",
                columns: new[] { "ActivityOccurrenceId", "Status" });

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
                name: "IX_Zones_LocationId_Name",
                table: "Zones",
                columns: new[] { "LocationId", "Name" },
                unique: true);
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
