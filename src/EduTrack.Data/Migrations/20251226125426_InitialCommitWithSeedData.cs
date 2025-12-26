using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommitWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthlyFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Groups_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Groups_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ForMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => new { x.StudentId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_StudentGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentGroups_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "CreatedAt", "IsDeleted", "Name", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Amir Temur St, 123", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Tashkent Main", "+998 71 207-50-50", null },
                    { 2, "Bodomzor St, 45", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Tashkent East", "+998 71 207-60-60", null },
                    { 3, "Registan Sq, 10", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Samarkand Branch", "+998 66 233-00-00", null }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "FirstName", "IsDeleted", "LastName", "ParentPhoneNumber", "PhoneNumber", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Tashkent", new DateTime(2005, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali", false, "Abdullayev", "+998 90 201-11-11", "+998 91 101-11-11", null },
                    { 2, "Tashkent", new DateTime(2004, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zarina", false, "Karimova", "+998 90 202-11-11", "+998 91 102-11-11", null },
                    { 3, "Tashkent", new DateTime(2005, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jamoliddin", false, "Rahimov", "+998 90 203-11-11", "+998 91 103-11-11", null },
                    { 4, "Tashkent", new DateTime(2003, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sitora", false, "Yusupova", "+998 90 204-11-11", "+998 91 104-11-11", null },
                    { 5, "Tashkent", new DateTime(2004, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mirjalol", false, "Mirmirov", "+998 90 205-11-11", "+998 91 105-11-11", null },
                    { 6, "Tashkent", new DateTime(2005, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dilnoza", false, "Sattarova", "+998 90 206-11-11", "+998 91 106-11-11", null },
                    { 7, "Tashkent", new DateTime(2004, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Navruzbek", false, "Kamolov", "+998 90 207-11-11", "+998 91 107-11-11", null },
                    { 8, "Tashkent", new DateTime(2005, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gulnara", false, "Azimova", "+998 90 208-11-11", "+998 91 108-11-11", null },
                    { 9, "Tashkent", new DateTime(2003, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakhrom", false, "Sharipov", "+998 90 209-11-11", "+998 91 109-11-11", null },
                    { 10, "Tashkent", new DateTime(2004, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nozim", false, "Abdullayev", "+998 90 210-11-11", "+998 91 110-11-11", null },
                    { 11, "Tashkent", new DateTime(2005, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marya", false, "Rustamova", "+998 90 211-11-11", "+998 91 111-11-11", null },
                    { 12, "Tashkent", new DateTime(2004, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Timur", false, "Nasimov", "+998 90 212-11-11", "+998 91 112-11-11", null },
                    { 13, "Tashkent", new DateTime(2005, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yasmin", false, "Ergasheva", "+998 90 213-11-11", "+998 91 113-11-11", null },
                    { 14, "Tashkent", new DateTime(2003, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fazliddin", false, "Qurbonov", "+998 90 214-11-11", "+998 91 114-11-11", null },
                    { 15, "Tashkent", new DateTime(2004, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Umida", false, "Sultonova", "+998 90 215-11-11", "+998 91 115-11-11", null },
                    { 16, "Tashkent", new DateTime(2005, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maxim", false, "Petrov", "+998 90 216-11-11", "+998 91 116-11-11", null },
                    { 17, "Tashkent", new DateTime(2004, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Irina", false, "Volkova", "+998 90 217-11-11", "+998 91 117-11-11", null },
                    { 18, "Tashkent", new DateTime(2005, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dmitri", false, "Sokolov", "+998 90 218-11-11", "+998 91 118-11-11", null },
                    { 19, "Tashkent", new DateTime(2004, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekaterina", false, "Kuznetsova", "+998 90 219-11-11", "+998 91 119-11-11", null },
                    { 20, "Tashkent", new DateTime(2005, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vladimir", false, "Smirnov", "+998 90 220-11-11", "+998 91 120-11-11", null },
                    { 21, "Tashkent", new DateTime(2004, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Olga", false, "Ivanova", "+998 90 221-11-11", "+998 91 121-11-11", null },
                    { 22, "Samarkand", new DateTime(2005, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sergei", false, "Lebedev", "+998 90 222-11-11", "+998 91 122-11-11", null },
                    { 23, "Samarkand", new DateTime(2004, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Natalia", false, "Morozova", "+998 90 223-11-11", "+998 91 123-11-11", null },
                    { 24, "Samarkand", new DateTime(2005, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andrey", false, "Orlov", "+998 90 224-11-11", "+998 91 124-11-11", null },
                    { 25, "Samarkand", new DateTime(2004, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Valentina", false, "Popova", "+998 90 225-11-11", "+998 91 125-11-11", null },
                    { 26, "Samarkand", new DateTime(2005, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mahmud", false, "Hakimov", "+998 90 226-11-11", "+998 91 126-11-11", null },
                    { 27, "Samarkand", new DateTime(2004, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Svetlana", false, "Sorokin", "+998 90 227-11-11", "+998 91 127-11-11", null },
                    { 28, "Samarkand", new DateTime(2005, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kamol", false, "Fayziev", "+998 90 228-11-11", "+998 91 128-11-11", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsDeleted", "LastName", "PasswordHash", "PhoneNumber", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@edutrack.uz", "John", false, "Smith", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 100-00-00", 0, null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fatima@edutrack.uz", "Fatima", false, "Karimova", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 111-11-11", 1, null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alisher@edutrack.uz", "Alisher", false, "Juraev", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 222-22-22", 1, null },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sardor@edutrack.uz", "Sardor", false, "Rashidov", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 333-33-33", 1, null },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nafisa@edutrack.uz", "Nafisa", false, "Abdullayeva", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 444-44-44", 1, null },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "oleg@edutrack.uz", "Oleg", false, "Petrov", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 555-55-55", 1, null },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nozima@edutrack.uz", "Nozima", false, "Iskandarova", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 666-66-66", 1, null },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rustam@edutrack.uz", "Rustam", false, "Nosirov", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 777-77-77", 1, null },
                    { 9, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "yulduz@edutrack.uz", "Yulduz", false, "Karimova", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 888-88-88", 1, null },
                    { 10, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dilshod@edutrack.uz", "Dilshod", false, "Akramov", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 999-99-99", 1, null },
                    { 11, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "elena@edutrack.uz", "Elena", false, "Volkova", "$2a$11$abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrst", "+998 90 101-01-01", 1, null },
                    { 12, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "gulnoza@edutrack.uz", "Gulnoza", false, "Abdullayeva", "Manager@123", "+998 91 100-00-00", 2, null },
                    { 13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aziz@edutrack.uz", "Aziz", false, "Khusnutdinov", "Manager@123", "+998 91 200-00-00", 2, null },
                    { 14, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "lola@edutrack.uz", "Lola", false, "Mirjalieva", "Manager@123", "+998 91 300-00-00", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "BranchId", "Capacity", "CreatedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, 30, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main classroom", false, "Room A1", null },
                    { 2, 1, 25, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medium classroom", false, "Room A2", null },
                    { 3, 1, 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Small classroom", false, "Room A3", null },
                    { 4, 2, 30, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main classroom", false, "Room B1", null },
                    { 5, 2, 25, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medium classroom", false, "Room B2", null },
                    { 6, 2, 20, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Small classroom", false, "Room B3", null },
                    { 7, 3, 30, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main classroom", false, "Room C1", null },
                    { 8, 3, 25, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medium classroom", false, "Room C2", null }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "BranchId", "CreatedAt", "Description", "EndDate", "IsDeleted", "MonthlyFee", "Name", "RoomId", "StartDate", "Subject", "TeacherId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beginner level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 500000m, "English A1 - Morning", 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 2, null },
                    { 2, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elementary level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 500000m, "English A2 - Evening", 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 3, null },
                    { 3, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intermediate level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 600000m, "English B1", 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 4, null },
                    { 4, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beginner level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 500000m, "English A1", 4, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 5, null },
                    { 5, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intermediate level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 600000m, "English B1", 5, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 6, null },
                    { 6, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Advanced level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 700000m, "English C1", 6, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 7, null },
                    { 7, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beginner level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 500000m, "English A1", 7, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 8, null },
                    { 8, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intermediate level English", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 600000m, "English B1", 8, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "English", 9, null }
                });

            migrationBuilder.InsertData(
                table: "StudentGroups",
                columns: new[] { "GroupId", "StudentId", "CreatedAt", "Id", "IsDeleted", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, null },
                    { 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, null },
                    { 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, null },
                    { 1, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, false, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_GroupId",
                table: "Attendances",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BranchId",
                table: "Groups",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_RoomId",
                table: "Groups",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_GroupId",
                table: "Payments",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StudentId",
                table: "Payments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BranchId",
                table: "Rooms",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_GroupId",
                table: "StudentGroups",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
