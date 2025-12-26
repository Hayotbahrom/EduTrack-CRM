using EduTrack.Domain.Entities;
using EduTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace EduTrack.Data.DBContexts;

public class EduDbContext : DbContext
{
    public EduDbContext(DbContextOptions<EduDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureRelationships(modelBuilder);

        SeedBranches(modelBuilder);
        SeedRooms(modelBuilder);
        SeedUsers(modelBuilder);
        SeedGroups(modelBuilder);
        SeedStudents(modelBuilder);
        SeedStudentGroups(modelBuilder);
        SeedPayments(modelBuilder);
        SeedAttendances(modelBuilder);


    }
    private static void SeedBranches(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>().HasData(
            new Branch { Id = 1, Name = "Tashkent Main", Address = "Amir Temur St, 123", PhoneNumber = "+998 71 207-50-50" },
            new Branch { Id = 2, Name = "Tashkent East", Address = "Bodomzor St, 45", PhoneNumber = "+998 71 207-60-60" },
            new Branch { Id = 3, Name = "Samarkand Branch", Address = "Registan Sq, 10", PhoneNumber = "+998 66 233-00-00" }
        );
    }

    private static void SeedRooms(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = 1, BranchId = 1, Name = "Room A1", Capacity = 30, Description = "Main classroom" },
            new Room { Id = 2, BranchId = 1, Name = "Room A2", Capacity = 25, Description = "Medium classroom" },
            new Room { Id = 3, BranchId = 1, Name = "Room A3", Capacity = 20, Description = "Small classroom" },
            new Room { Id = 4, BranchId = 2, Name = "Room B1", Capacity = 30, Description = "Main classroom" },
            new Room { Id = 5, BranchId = 2, Name = "Room B2", Capacity = 25, Description = "Medium classroom" },
            new Room { Id = 6, BranchId = 2, Name = "Room B3", Capacity = 20, Description = "Small classroom" },
            new Room { Id = 7, BranchId = 3, Name = "Room C1", Capacity = 30, Description = "Main classroom" },
            new Room { Id = 8, BranchId = 3, Name = "Room C2", Capacity = 25, Description = "Medium classroom" }
        );
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        var users = new List<User>
        {
            // Admin (1)
            new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john@edutrack.uz",
                PhoneNumber = "+998 90 100-00-00",
                Role = 0, // Admin
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                CreatedAt = DateTime.Now
            },
            // Teachers (10)
            new User { Id = 2, FirstName = "Fatima", LastName = "Karimova", Email = "fatima@edutrack.uz", PhoneNumber = "+998 90 111-11-11", Role = (UserRole)1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 3, FirstName = "Alisher", LastName = "Juraev", Email = "alisher@edutrack.uz", PhoneNumber = "+998 90 222-22-22", Role = (UserRole)1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 4, FirstName = "Sardor", LastName = "Rashidov", Email = "sardor@edutrack.uz", PhoneNumber = "+998 90 333-33-33", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 5, FirstName = "Nafisa", LastName = "Abdullayeva", Email = "nafisa@edutrack.uz", PhoneNumber = "+998 90 444-44-44", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 6, FirstName = "Oleg", LastName = "Petrov", Email = "oleg@edutrack.uz", PhoneNumber = "+998 90 555-55-55", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 7, FirstName = "Nozima", LastName = "Iskandarova", Email = "nozima@edutrack.uz", PhoneNumber = "+998 90 666-66-66", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 8, FirstName = "Rustam", LastName = "Nosirov", Email = "rustam@edutrack.uz", PhoneNumber = "+998 90 777-77-77", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 9, FirstName = "Yulduz", LastName = "Karimova", Email = "yulduz@edutrack.uz", PhoneNumber = "+998 90 888-88-88", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 10, FirstName = "Dilshod", LastName = "Akramov", Email = "dilshod@edutrack.uz", PhoneNumber = "+998 90 999-99-99", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            new User { Id = 11, FirstName = "Elena", LastName = "Volkova", Email = "elena@edutrack.uz", PhoneNumber = "+998 90 101-01-01", Role = (UserRole) 1, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"), CreatedAt = DateTime.Now },
            // Managers (3)
            new User { Id = 12, FirstName = "Gulnoza", LastName = "Abdullayeva", Email = "gulnoza@edutrack.uz", PhoneNumber = "+998 91 100-00-00", Role = (UserRole)2, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), CreatedAt = DateTime.Now },
            new User { Id = 13, FirstName = "Aziz", LastName = "Khusnutdinov", Email = "aziz@edutrack.uz", PhoneNumber = "+998 91 200-00-00", Role = (UserRole) 2, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), CreatedAt = DateTime.Now },
            new User { Id = 14, FirstName = "Lola", LastName = "Mirjalieva", Email = "lola@edutrack.uz", PhoneNumber = "+998 91 300-00-00", Role = (UserRole) 2, PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), CreatedAt = DateTime.Now }
        };

        modelBuilder.Entity<User>().HasData(users);
    }

    private static void SeedGroups(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasData(
            new Group { Id = 1, BranchId = 1, RoomId = 1, TeacherId = 2, Name = "English A1 - Morning", Subject = "English", Description = "Beginner level English", MonthlyFee = 500000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 2, BranchId = 1, RoomId = 2, TeacherId = 3, Name = "English A2 - Evening", Subject = "English", Description = "Elementary level English", MonthlyFee = 500000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 3, BranchId = 1, RoomId = 3, TeacherId = 4, Name = "English B1", Subject = "English", Description = "Intermediate level English", MonthlyFee = 600000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 4, BranchId = 2, RoomId = 4, TeacherId = 5, Name = "English A1", Subject = "English", Description = "Beginner level English", MonthlyFee = 500000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 5, BranchId = 2, RoomId = 5, TeacherId = 6, Name = "English B1", Subject = "English", Description = "Intermediate level English", MonthlyFee = 600000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 6, BranchId = 2, RoomId = 6, TeacherId = 7, Name = "English C1", Subject = "English", Description = "Advanced level English", MonthlyFee = 700000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 7, BranchId = 3, RoomId = 7, TeacherId = 8, Name = "English A1", Subject = "English", Description = "Beginner level English", MonthlyFee = 500000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) },
            new Group { Id = 8, BranchId = 3, RoomId = 8, TeacherId = 9, Name = "English B1", Subject = "English", Description = "Intermediate level English", MonthlyFee = 600000, StartDate = new DateTime(2025, 1, 15), EndDate = new DateTime(2025, 6, 15) }
        );
    }

    private static void SeedStudents(ModelBuilder modelBuilder)
    {
        var students = new List<Student>
        {
            new Student { Id = 1, FirstName = "Ali", LastName = "Abdullayev", BirthDate = new DateTime(2005, 5, 15), PhoneNumber = "+998 91 101-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 201-11-11" },
            new Student { Id = 2, FirstName = "Zarina", LastName = "Karimova", BirthDate = new DateTime(2004, 8, 22), PhoneNumber = "+998 91 102-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 202-11-11" },
            new Student { Id = 3, FirstName = "Jamoliddin", LastName = "Rahimov", BirthDate = new DateTime(2005, 3, 10), PhoneNumber = "+998 91 103-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 203-11-11" },
            new Student { Id = 4, FirstName = "Sitora", LastName = "Yusupova", BirthDate = new DateTime(2003, 12, 5), PhoneNumber = "+998 91 104-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 204-11-11" },
            new Student { Id = 5, FirstName = "Mirjalol", LastName = "Mirmirov", BirthDate = new DateTime(2004, 6, 18), PhoneNumber = "+998 91 105-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 205-11-11" },
            new Student { Id = 6, FirstName = "Dilnoza", LastName = "Sattarova", BirthDate = new DateTime(2005, 9, 25), PhoneNumber = "+998 91 106-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 206-11-11" },
            new Student { Id = 7, FirstName = "Navruzbek", LastName = "Kamolov", BirthDate = new DateTime(2004, 2, 14), PhoneNumber = "+998 91 107-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 207-11-11" },
            new Student { Id = 8, FirstName = "Gulnara", LastName = "Azimova", BirthDate = new DateTime(2005, 7, 8), PhoneNumber = "+998 91 108-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 208-11-11" },
            new Student { Id = 9, FirstName = "Bakhrom", LastName = "Sharipov", BirthDate = new DateTime(2003, 11, 30), PhoneNumber = "+998 91 109-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 209-11-11" },
            new Student { Id = 10, FirstName = "Nozim", LastName = "Abdullayev", BirthDate = new DateTime(2004, 4, 17), PhoneNumber = "+998 91 110-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 210-11-11" },
            new Student { Id = 11, FirstName = "Marya", LastName = "Rustamova", BirthDate = new DateTime(2005, 1, 20), PhoneNumber = "+998 91 111-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 211-11-11" },
            new Student { Id = 12, FirstName = "Timur", LastName = "Nasimov", BirthDate = new DateTime(2004, 10, 11), PhoneNumber = "+998 91 112-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 212-11-11" },
            new Student { Id = 13, FirstName = "Yasmin", LastName = "Ergasheva", BirthDate = new DateTime(2005, 2, 28), PhoneNumber = "+998 91 113-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 213-11-11" },
            new Student { Id = 14, FirstName = "Fazliddin", LastName = "Qurbonov", BirthDate = new DateTime(2003, 9, 5), PhoneNumber = "+998 91 114-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 214-11-11" },
            new Student { Id = 15, FirstName = "Umida", LastName = "Sultonova", BirthDate = new DateTime(2004, 12, 3), PhoneNumber = "+998 91 115-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 215-11-11" },
            new Student { Id = 16, FirstName = "Maxim", LastName = "Petrov", BirthDate = new DateTime(2005, 4, 12), PhoneNumber = "+998 91 116-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 216-11-11" },
            new Student { Id = 17, FirstName = "Irina", LastName = "Volkova", BirthDate = new DateTime(2004, 7, 19), PhoneNumber = "+998 91 117-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 217-11-11" },
            new Student { Id = 18, FirstName = "Dmitri", LastName = "Sokolov", BirthDate = new DateTime(2005, 8, 27), PhoneNumber = "+998 91 118-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 218-11-11" },
            new Student { Id = 19, FirstName = "Ekaterina", LastName = "Kuznetsova", BirthDate = new DateTime(2004, 9, 3), PhoneNumber = "+998 91 119-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 219-11-11" },
            new Student { Id = 20, FirstName = "Vladimir", LastName = "Smirnov", BirthDate = new DateTime(2005, 10, 15), PhoneNumber = "+998 91 120-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 220-11-11" },
            new Student { Id = 21, FirstName = "Olga", LastName = "Ivanova", BirthDate = new DateTime(2004, 11, 22), PhoneNumber = "+998 91 121-11-11", Address = "Tashkent", ParentPhoneNumber = "+998 90 221-11-11" },
            new Student { Id = 22, FirstName = "Sergei", LastName = "Lebedev", BirthDate = new DateTime(2005, 6, 8), PhoneNumber = "+998 91 122-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 222-11-11" },
            new Student { Id = 23, FirstName = "Natalia", LastName = "Morozova", BirthDate = new DateTime(2004, 1, 14), PhoneNumber = "+998 91 123-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 223-11-11" },
            new Student { Id = 24, FirstName = "Andrey", LastName = "Orlov", BirthDate = new DateTime(2005, 3, 25), PhoneNumber = "+998 91 124-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 224-11-11" },
            new Student { Id = 25, FirstName = "Valentina", LastName = "Popova", BirthDate = new DateTime(2004, 5, 30), PhoneNumber = "+998 91 125-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 225-11-11" },
            new Student { Id = 26, FirstName = "Mahmud", LastName = "Hakimov", BirthDate = new DateTime(2005, 11, 9), PhoneNumber = "+998 91 126-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 226-11-11" },
            new Student { Id = 27, FirstName = "Svetlana", LastName = "Sorokin", BirthDate = new DateTime(2004, 3, 18), PhoneNumber = "+998 91 127-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 227-11-11" },
            new Student { Id = 28, FirstName = "Kamol", LastName = "Fayziev", BirthDate = new DateTime(2005, 12, 7), PhoneNumber = "+998 91 128-11-11", Address = "Samarkand", ParentPhoneNumber = "+998 90 228-11-11" }
        };

        modelBuilder.Entity<Student>().HasData(students);
    }

    private static void SeedStudentGroups(ModelBuilder modelBuilder)
    {
        var enrollments = new List<StudentGroup>
        {
            // Group 1 (4)
            new StudentGroup { StudentId = 1, GroupId = 1 },
            new StudentGroup { StudentId = 2, GroupId = 1 },
            new StudentGroup { StudentId = 3, GroupId = 1 },
            new StudentGroup { StudentId = 4, GroupId = 1 },
            // Group 2 (4)
            new StudentGroup { StudentId = 5, GroupId = 2 },
            new StudentGroup { StudentId = 6, GroupId = 2 },
            new StudentGroup { StudentId = 7, GroupId = 2 },
            new StudentGroup { StudentId = 8, GroupId = 2 },
            // Group 3 (3)
            new StudentGroup { StudentId = 9, GroupId = 3 },
            new StudentGroup { StudentId = 10, GroupId = 3 },
            new StudentGroup { StudentId = 11, GroupId = 3 },
            // Group 4 (4)
            new StudentGroup { StudentId = 12, GroupId = 4 },
            new StudentGroup { StudentId = 13, GroupId = 4 },
            new StudentGroup { StudentId = 14, GroupId = 4 },
            new StudentGroup { StudentId = 15, GroupId = 4 },
            // Group 5 (3)
            new StudentGroup { StudentId = 16, GroupId = 5 },
            new StudentGroup { StudentId = 17, GroupId = 5 },
            new StudentGroup { StudentId = 18, GroupId = 5 },
            // Group 6 (3)
            new StudentGroup { StudentId = 19, GroupId = 6 },
            new StudentGroup { StudentId = 20, GroupId = 6 },
            new StudentGroup { StudentId = 21, GroupId = 6 },
            // Group 7 (4)
            new StudentGroup { StudentId = 22, GroupId = 7 },
            new StudentGroup { StudentId = 23, GroupId = 7 },
            new StudentGroup { StudentId = 24, GroupId = 7 },
            new StudentGroup { StudentId = 25, GroupId = 7 },
            // Group 8 (3)
            new StudentGroup { StudentId = 26, GroupId = 8 },
            new StudentGroup { StudentId = 27, GroupId = 8 },
            new StudentGroup { StudentId = 28, GroupId = 8 }
        };

        modelBuilder.Entity<StudentGroup>().HasData(enrollments);
    }

    private static void SeedPayments(ModelBuilder modelBuilder)
    {
        var payments = new List<Payment>();
        int paymentId = 1;

        // Group 1-3: 3 months, Group 4+: 1-2 months
        paymentId = AddPaymentsForGroup(payments, paymentId, groupId: 1, studentIds: new[] { 1, 2, 3, 4 }, months: 3, fee: 500000);
        paymentId = AddPaymentsForGroup(payments, paymentId, groupId: 2, studentIds: new[] { 5, 6, 7, 8 }, months: 2, fee: 500000);
        paymentId = AddPaymentsForGroup(payments, paymentId, groupId: 3, studentIds: new[] { 9, 10, 11 }, months: 1, fee: 600000);
        paymentId = AddPaymentsForGroup(payments, paymentId, groupId: 4, studentIds: new[] { 12, 13, 14, 15 }, months: 1, fee: 500000);

        modelBuilder.Entity<Payment>().HasData(payments);
    }

    private static int AddPaymentsForGroup(List<Payment> payments, int paymentId, int groupId, int[] studentIds, int months, decimal fee)
    {
        var monthNames = new[] { "January", "February", "March" };

        foreach (var studentId in studentIds)
        {
            for (int m = 0; m < months; m++)
            {
                payments.Add(new Payment
                {
                    Id = paymentId++,
                    StudentId = studentId,
                    GroupId = groupId,
                    Amount = fee,
                    PaymentDate = m < 2 ? new DateTime(2025, m + 1, 20) : null,
                    ForMoth = monthNames[m],
                    Description = $"Payment for {monthNames[m]} 2025",
                    PaymentMethod = m < 2 ? "Cash" : null
                });
            }
        }

        return paymentId;
    }

    private static void SeedAttendances(ModelBuilder modelBuilder)
    {
        var attendances = new List<Attendance>();
        int attendanceId = 1;

        var groupStudents = new Dictionary<int, int[]>
        {
            { 1, new[] { 1, 2, 3, 4 } },
            { 2, new[] { 5, 6, 7, 8 } },
            { 3, new[] { 9, 10, 11 } },
            { 4, new[] { 12, 13, 14, 15 } },
            { 5, new[] { 16, 17, 18 } },
            { 6, new[] { 19, 20, 21 } },
            { 7, new[] { 22, 23, 24, 25 } },
            { 8, new[] { 26, 27, 28 } }
        };

        var attendanceDates = new[] { 15, 17, 19, 22, 24 };

        foreach (var group in groupStudents)
        {
            foreach (var studentId in group.Value)
            {
                foreach (var day in attendanceDates)
                {
                    attendances.Add(new Attendance
                    {
                        Id = attendanceId++,
                        StudentId = studentId,
                        GroupId = group.Key,
                        Date = new DateTime(2025, 1, day),
                        IsPresent = (studentId + day) % 3 != 0,
                        Remarks = (studentId + day) % 3 == 0 ? "Absence" : null
                    });
                }
            }
        }

        modelBuilder.Entity<Attendance>().HasData(attendances);
    }
    private static void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // decimal precision configuration
        modelBuilder.Entity<Group>()
            .Property(g => g.MonthlyFee)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        // room - branch one-to-many
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Branch)
            .WithMany(b => b.Rooms)
            .HasForeignKey(r => r.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        // group - branch one-to-many
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Branch)
            .WithMany(b => b.Groups)
            .HasForeignKey(g => g.BranchId)
            .OnDelete(DeleteBehavior.NoAction);

        // group - room one-to-many
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Room)
            .WithMany(r => r.Groups)
            .HasForeignKey(g => g.RoomId)
            .OnDelete(DeleteBehavior.NoAction);


        // group - teacher (user) one-to-many
        modelBuilder.Entity<Group>()
            .HasOne(g => g.Teacher)
            .WithMany(u => u.Groups)
            .HasForeignKey(g => g.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<StudentGroup>()
            .HasKey(sg => new { sg.StudentId, sg.GroupId });

        modelBuilder.Entity<StudentGroup>()
            .HasOne(sg => sg.Student)
            .WithMany(s => s.StudentGroups)
            .HasForeignKey(sg => sg.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentGroup>()
            .HasOne(sg => sg.Group)
            .WithMany(g => g.StudentGroups)
            .HasForeignKey(sg => sg.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // student attendance relationship
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Attendances)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        // group attendance relationship
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Group)
            .WithMany(g => g.Attendances)
            .HasForeignKey(a => a.GroupId)
            .OnDelete(DeleteBehavior.NoAction);

        // student payment relationship
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Student)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        // group payment relationship
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Group)
            .WithMany(g => g.Payments)
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}
