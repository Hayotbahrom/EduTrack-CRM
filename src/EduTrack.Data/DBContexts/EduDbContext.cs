using EduTrack.Domain.Entities;
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
