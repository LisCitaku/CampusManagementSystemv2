namespace CampusManagementSystem.Application.DTOs;

public class EnrollmentDto
{
    public Guid EnrollmentId { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class CreateEnrollmentDto
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}

public class UpdateEnrollmentStatusDto
{
    public string Status { get; set; } = string.Empty;
}
