namespace CampusManagementSystem.Application.DTOs;

public class IssueReportDto
{
    public Guid IssueId { get; set; }
    public Guid FacilityId { get; set; }
    public Guid CreatedById { get; set; }
    public Guid? AssignedToId { get; set; }
    public string FacilityType { get; set; } = string.Empty;
    public string CreatedByName { get; set; } = string.Empty;
    public string? AssignedToName { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateIssueReportDto
{
    public Guid FacilityId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
}

public class UpdateIssueReportDto
{
    public Guid? AssignedToId { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
}
