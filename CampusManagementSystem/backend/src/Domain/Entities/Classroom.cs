namespace CampusManagementSystem.Domain.Entities;

public class Classroom
{
    public Guid ClassroomId { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
