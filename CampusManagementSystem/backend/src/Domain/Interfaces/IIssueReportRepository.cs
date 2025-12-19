using CampusManagementSystem.Domain.Entities;

namespace CampusManagementSystem.Domain.Interfaces;

public interface IIssueReportRepository : IRepository<IssueReport>
{
    Task<IEnumerable<IssueReport>> GetByFacilityIdAsync(Guid facilityId);
    Task<IEnumerable<IssueReport>> GetByCreatedByIdAsync(Guid userId);
    Task<IEnumerable<IssueReport>> GetAssignedToStaffAsync(Guid staffId);
    Task<IEnumerable<IssueReport>> GetOpenIssuesAsync();
}
