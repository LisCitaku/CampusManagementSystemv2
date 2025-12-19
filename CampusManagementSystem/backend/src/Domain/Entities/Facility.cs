using CampusManagementSystem.Domain.Enums;

namespace CampusManagementSystem.Domain.Entities;

public class Facility
{
    public Guid FacilityId { get; set; } = Guid.NewGuid();
    public string FacilityType { get; set; } = string.Empty;
    public FacilityStatus Status { get; set; } = FacilityStatus.Available;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<IssueReport> IssueReports { get; set; } = new List<IssueReport>();
}
