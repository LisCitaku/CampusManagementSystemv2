namespace CampusManagementSystem.Application.DTOs;

public class StudentDto : UserDto
{
    public string StudentNumber { get; set; } = string.Empty;
    public int YearOfStudy { get; set; }
}

public class CreateStudentDto : CreateUserDto
{
    public string StudentNumber { get; set; } = string.Empty;
    public int YearOfStudy { get; set; }
}

public class UpdateStudentDto : UpdateUserDto
{
    public int? YearOfStudy { get; set; }
}
