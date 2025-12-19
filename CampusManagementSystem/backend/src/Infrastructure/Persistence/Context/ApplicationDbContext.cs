using CampusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<IssueReport> IssueReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure inheritance
        modelBuilder.Entity<User>().HasDiscriminator<string>("user_type")
            .HasValue<User>("User")
            .HasValue<Student>("Student")
            .HasValue<Staff>("Staff");

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedIssues)
            .WithOne(ir => ir.CreatedBy)
            .HasForeignKey(ir => ir.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Course configuration
        modelBuilder.Entity<Course>()
            .HasKey(c => c.CourseId);

        modelBuilder.Entity<Course>()
            .Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(255);

        // Enrollment configuration
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => e.EnrollmentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique()
            .HasName("IX_Unique_StudentCourse");

        // Classroom configuration
        modelBuilder.Entity<Classroom>()
            .HasKey(c => c.ClassroomId);

        modelBuilder.Entity<Classroom>()
            .Property(c => c.Type)
            .IsRequired()
            .HasMaxLength(100);

        // Reservation configuration
        modelBuilder.Entity<Reservation>()
            .HasKey(r => r.ReservationId);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Classroom)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Staff)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.StaffId)
            .OnDelete(DeleteBehavior.Cascade);

        // Facility configuration
        modelBuilder.Entity<Facility>()
            .HasKey(f => f.FacilityId);

        modelBuilder.Entity<Facility>()
            .Property(f => f.FacilityType)
            .IsRequired()
            .HasMaxLength(100);

        // IssueReport configuration
        modelBuilder.Entity<IssueReport>()
            .HasKey(ir => ir.IssueId);

        modelBuilder.Entity<IssueReport>()
            .HasOne(ir => ir.Facility)
            .WithMany(f => f.IssueReports)
            .HasForeignKey(ir => ir.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IssueReport>()
            .HasOne(ir => ir.AssignedTo)
            .WithMany(s => s.AssignedIssues)
            .HasForeignKey(ir => ir.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<IssueReport>()
            .Property(ir => ir.Description)
            .IsRequired()
            .HasMaxLength(1000);

        // Add indexes for performance
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => e.CourseId);

        modelBuilder.Entity<Reservation>()
            .HasIndex(r => r.ClassroomId);

        modelBuilder.Entity<Reservation>()
            .HasIndex(r => r.StaffId);

        modelBuilder.Entity<IssueReport>()
            .HasIndex(ir => ir.FacilityId);

        modelBuilder.Entity<IssueReport>()
            .HasIndex(ir => ir.CreatedById);

        modelBuilder.Entity<IssueReport>()
            .HasIndex(ir => ir.Status);
    }
}
