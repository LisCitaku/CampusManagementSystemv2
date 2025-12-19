namespace CampusManagementSystem.Application.DTOs;

public class ClassroomDto
{
    public Guid ClassroomId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

public class CreateClassroomDto
{
    public string Type { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

public class UpdateClassroomDto
{
    public string? Type { get; set; }
    public int? Capacity { get; set; }
}
