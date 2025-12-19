namespace CampusManagementSystem.Domain.Entities;

public class Staff : User
{
    public string StaffType { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<IssueReport> AssignedIssues { get; set; } = new List<IssueReport>();
}
