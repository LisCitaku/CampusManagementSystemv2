namespace CampusManagementSystem.Application.DTOs;

public class ReservationDto
{
    public Guid ReservationId { get; set; }
    public Guid ClassroomId { get; set; }
    public Guid StaffId { get; set; }
    public string ClassroomType { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateReservationDto
{
    public Guid ClassroomId { get; set; }
    public Guid StaffId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class UpdateReservationDto
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Status { get; set; }
}
