using CampusManagementSystem.Domain.Enums;

namespace CampusManagementSystem.Domain.Entities;

public class Reservation
{
    public Guid ReservationId { get; set; } = Guid.NewGuid();
    public Guid ClassroomId { get; set; }
    public Guid StaffId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Classroom? Classroom { get; set; }
    public virtual Staff? Staff { get; set; }
}
