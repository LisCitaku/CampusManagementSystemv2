namespace CampusManagementSystem.Domain.Entities;

public class Course
{
    public Guid CourseId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public int CreditPoints { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
