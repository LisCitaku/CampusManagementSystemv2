namespace CampusManagementSystem.Application.DTOs;

public class StaffDto : UserDto
{
    public string StaffType { get; set; } = string.Empty;
}

public class CreateStaffDto : CreateUserDto
{
    public string StaffType { get; set; } = string.Empty;
}

public class UpdateStaffDto : UpdateUserDto
{
    public string? StaffType { get; set; }
}
