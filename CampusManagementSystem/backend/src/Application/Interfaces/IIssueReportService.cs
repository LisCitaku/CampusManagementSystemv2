using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IIssueReportService
{
    Task<IssueReportDto?> GetIssueByIdAsync(Guid issueId);
    Task<IEnumerable<IssueReportDto>> GetFacilityIssuesAsync(Guid facilityId);
    Task<IEnumerable<IssueReportDto>> GetOpenIssuesAsync();
    Task<IEnumerable<IssueReportDto>> GetAssignedToStaffAsync(Guid staffId);
    Task<IssueReportDto> CreateIssueAsync(CreateIssueReportDto createIssueDto, Guid userId);
    Task<IssueReportDto> UpdateIssueAsync(Guid issueId, UpdateIssueReportDto updateDto);
    Task<bool> DeleteIssueAsync(Guid issueId);
}
