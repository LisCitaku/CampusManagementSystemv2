namespace CampusManagementSystem.Application.DTOs;

public class CourseDto
{
    public Guid CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CreditPoints { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentEnrollments { get; set; }
}

public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public int CreditPoints { get; set; }
    public int MaxCapacity { get; set; }
}

public class UpdateCourseDto
{
    public string? Title { get; set; }
    public int? CreditPoints { get; set; }
    public int? MaxCapacity { get; set; }
}
