using CampusManagementSystem.Domain.Enums;

namespace CampusManagementSystem.Domain.Entities;

public class IssueReport
{
    public Guid IssueId { get; set; } = Guid.NewGuid();
    public Guid FacilityId { get; set; }
    public Guid CreatedById { get; set; }
    public Guid? AssignedToId { get; set; }
    public string Description { get; set; } = string.Empty;
    public IssuePriority Priority { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    // Navigation properties
    public virtual Facility? Facility { get; set; }
    public virtual User? CreatedBy { get; set; }
    public virtual Staff? AssignedTo { get; set; }
}
