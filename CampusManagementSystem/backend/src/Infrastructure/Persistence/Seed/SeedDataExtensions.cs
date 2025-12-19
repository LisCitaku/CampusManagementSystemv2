using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Enums;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Persistence.Seed;

public static class SeedDataExtensions
{
    public static async Task SeedInitialDataAsync(this ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync())
            return; // Already seeded

        // Create Admin User
        var adminUser = new User
        {
            UserId = Guid.NewGuid(),
            Name = "Admin User",
            Email = "admin@campus.edu",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            RoleType = RoleType.Admin,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();

        // Create Students
        var students = new List<Student>
        {
            new Student
            {
                UserId = Guid.NewGuid(),
                Name = "Alice Johnson",
                Email = "alice.johnson@student.edu",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                StudentNumber = "STU001",
                YearOfStudy = 2,
                RoleType = RoleType.Student,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Student
            {
                UserId = Guid.NewGuid(),
                Name = "Bob Smith",
                Email = "bob.smith@student.edu",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                StudentNumber = "STU002",
                YearOfStudy = 1,
                RoleType = RoleType.Student,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Students.AddRange(students);
        await context.SaveChangesAsync();

        // Create Staff
        var staff = new List<Staff>
        {
            new Staff
            {
                UserId = Guid.NewGuid(),
                Name = "Dr. Sarah Williams",
                Email = "sarah.williams@faculty.edu",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                StaffType = "Faculty",
                RoleType = RoleType.Staff,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Staff
            {
                UserId = Guid.NewGuid(),
                Name = "John Davis",
                Email = "john.davis@maintenance.edu",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                StaffType = "Maintenance",
                RoleType = RoleType.Staff,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Staff.AddRange(staff);
        await context.SaveChangesAsync();

        // Create Courses
        var courses = new List<Course>
        {
            new Course
            {
                CourseId = Guid.NewGuid(),
                Title = "Introduction to Computer Science",
                CreditPoints = 3,
                MaxCapacity = 50,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                CourseId = Guid.NewGuid(),
                Title = "Advanced Mathematics",
                CreditPoints = 4,
                MaxCapacity = 40,
                CreatedAt = DateTime.UtcNow
            },
            new Course
            {
                CourseId = Guid.NewGuid(),
                Title = "Physics Fundamentals",
                CreditPoints = 3,
                MaxCapacity = 35,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        // Create Classrooms
        var classrooms = new List<Classroom>
        {
            new Classroom
            {
                ClassroomId = Guid.NewGuid(),
                Type = "Lecture Hall",
                Capacity = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Classroom
            {
                ClassroomId = Guid.NewGuid(),
                Type = "Lab",
                Capacity = 30,
                CreatedAt = DateTime.UtcNow
            },
            new Classroom
            {
                ClassroomId = Guid.NewGuid(),
                Type = "Seminar Room",
                Capacity = 25,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Classrooms.AddRange(classrooms);
        await context.SaveChangesAsync();

        // Create Facilities
        var facilities = new List<Facility>
        {
            new Facility
            {
                FacilityId = Guid.NewGuid(),
                FacilityType = "Library",
                Status = FacilityStatus.Available,
                CreatedAt = DateTime.UtcNow
            },
            new Facility
            {
                FacilityId = Guid.NewGuid(),
                FacilityType = "Gym",
                Status = FacilityStatus.Available,
                CreatedAt = DateTime.UtcNow
            },
            new Facility
            {
                FacilityId = Guid.NewGuid(),
                FacilityType = "Cafeteria",
                Status = FacilityStatus.Available,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Facilities.AddRange(facilities);
        await context.SaveChangesAsync();

        // Create Enrollments
        var enrollments = new List<Enrollment>
        {
            new Enrollment
            {
                EnrollmentId = Guid.NewGuid(),
                StudentId = students[0].UserId,
                CourseId = courses[0].CourseId,
                Status = EnrollmentStatus.Active,
                Timestamp = DateTime.UtcNow
            },
            new Enrollment
            {
                EnrollmentId = Guid.NewGuid(),
                StudentId = students[0].UserId,
                CourseId = courses[1].CourseId,
                Status = EnrollmentStatus.Active,
                Timestamp = DateTime.UtcNow
            },
            new Enrollment
            {
                EnrollmentId = Guid.NewGuid(),
                StudentId = students[1].UserId,
                CourseId = courses[2].CourseId,
                Status = EnrollmentStatus.Pending,
                Timestamp = DateTime.UtcNow
            }
        };

        context.Enrollments.AddRange(enrollments);
        await context.SaveChangesAsync();

        // Create Reservations
        var reservations = new List<Reservation>
        {
            new Reservation
            {
                ReservationId = Guid.NewGuid(),
                ClassroomId = classrooms[0].ClassroomId,
                StaffId = staff[0].UserId,
                StartTime = DateTime.UtcNow.AddDays(1).Date.AddHours(9),
                EndTime = DateTime.UtcNow.AddDays(1).Date.AddHours(11),
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            },
            new Reservation
            {
                ReservationId = Guid.NewGuid(),
                ClassroomId = classrooms[1].ClassroomId,
                StaffId = staff[1].UserId,
                StartTime = DateTime.UtcNow.AddDays(2).Date.AddHours(14),
                EndTime = DateTime.UtcNow.AddDays(2).Date.AddHours(16),
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Reservations.AddRange(reservations);
        await context.SaveChangesAsync();

        // Create Issue Reports
        var issueReports = new List<IssueReport>
        {
            new IssueReport
            {
                IssueId = Guid.NewGuid(),
                FacilityId = facilities[0].FacilityId,
                CreatedById = students[0].UserId,
                AssignedToId = staff[1].UserId,
                Description = "Broken window in library main reading room",
                Priority = IssuePriority.High,
                Status = IssueStatus.InProgress,
                CreatedAt = DateTime.UtcNow
            },
            new IssueReport
            {
                IssueId = Guid.NewGuid(),
                FacilityId = facilities[1].FacilityId,
                CreatedById = adminUser.UserId,
                AssignedToId = null,
                Description = "Air conditioning not working properly",
                Priority = IssuePriority.Medium,
                Status = IssueStatus.Open,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.IssueReports.AddRange(issueReports);
        await context.SaveChangesAsync();
    }
}
