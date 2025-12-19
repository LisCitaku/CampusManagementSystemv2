using CampusManagementSystem.Domain.Enums;

namespace CampusManagementSystem.Domain.Entities;

public class Enrollment
{
    public Guid EnrollmentId { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public virtual Student? Student { get; set; }
    public virtual Course? Course { get; set; }
}
