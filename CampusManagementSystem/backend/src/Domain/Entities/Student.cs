namespace CampusManagementSystem.Domain.Entities;

public class Student : User
{
    public string StudentNumber { get; set; } = string.Empty;
    public int YearOfStudy { get; set; }

    // Navigation properties
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
