namespace CampusManagementSystem.Application.DTOs;

public class FacilityDto
{
    public Guid FacilityId { get; set; }
    public string FacilityType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CreateFacilityDto
{
    public string FacilityType { get; set; } = string.Empty;
}

public class UpdateFacilityDto
{
    public string? FacilityType { get; set; }
    public string? Status { get; set; }
}
