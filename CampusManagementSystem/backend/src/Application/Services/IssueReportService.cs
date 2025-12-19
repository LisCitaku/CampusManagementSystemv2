using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Enums;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class IssueReportService : IIssueReportService
{
    private readonly IIssueReportRepository _issueReportRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStaffRepository _staffRepository;

    public IssueReportService(IIssueReportRepository issueReportRepository, IFacilityRepository facilityRepository, IUserRepository userRepository, IStaffRepository staffRepository)
    {
        _issueReportRepository = issueReportRepository;
        _facilityRepository = facilityRepository;
        _userRepository = userRepository;
        _staffRepository = staffRepository;
    }

    public async Task<IssueReportDto?> GetIssueByIdAsync(Guid issueId)
    {
        var issue = await _issueReportRepository.GetByIdAsync(issueId);
        return issue == null ? null : await MapToIssueReportDtoAsync(issue);
    }

    public async Task<IEnumerable<IssueReportDto>> GetFacilityIssuesAsync(Guid facilityId)
    {
        var issues = await _issueReportRepository.GetByFacilityIdAsync(facilityId);
        var result = new List<IssueReportDto>();
        foreach (var issue in issues)
        {
            result.Add(await MapToIssueReportDtoAsync(issue));
        }
        return result;
    }

    public async Task<IEnumerable<IssueReportDto>> GetOpenIssuesAsync()
    {
        var issues = await _issueReportRepository.GetOpenIssuesAsync();
        var result = new List<IssueReportDto>();
        foreach (var issue in issues)
        {
            result.Add(await MapToIssueReportDtoAsync(issue));
        }
        return result;
    }

    public async Task<IEnumerable<IssueReportDto>> GetAssignedToStaffAsync(Guid staffId)
    {
        var issues = await _issueReportRepository.GetAssignedToStaffAsync(staffId);
        var result = new List<IssueReportDto>();
        foreach (var issue in issues)
        {
            result.Add(await MapToIssueReportDtoAsync(issue));
        }
        return result;
    }

    public async Task<IssueReportDto> CreateIssueAsync(CreateIssueReportDto createIssueDto, Guid userId)
    {
        var facility = await _facilityRepository.GetByIdAsync(createIssueDto.FacilityId);
        if (facility == null)
            throw new InvalidOperationException("Facility not found");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var issue = new IssueReport
        {
            IssueId = Guid.NewGuid(),
            FacilityId = createIssueDto.FacilityId,
            CreatedById = userId,
            Description = createIssueDto.Description,
            Priority = Enum.Parse<IssuePriority>(createIssueDto.Priority),
            Status = IssueStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        var createdIssue = await _issueReportRepository.AddAsync(issue);
        return await MapToIssueReportDtoAsync(createdIssue);
    }

    public async Task<IssueReportDto> UpdateIssueAsync(Guid issueId, UpdateIssueReportDto updateDto)
    {
        var issue = await _issueReportRepository.GetByIdAsync(issueId);
        if (issue == null)
            throw new InvalidOperationException("Issue not found");

        if (updateDto.AssignedToId.HasValue)
        {
            var staff = await _staffRepository.GetByIdAsync(updateDto.AssignedToId.Value);
            if (staff == null)
                throw new InvalidOperationException("Staff not found");
            issue.AssignedToId = updateDto.AssignedToId;
        }

        if (!string.IsNullOrEmpty(updateDto.Status))
        {
            issue.Status = Enum.Parse<IssueStatus>(updateDto.Status);
            if (updateDto.Status == "Resolved" || updateDto.Status == "Closed")
                issue.ResolvedAt = DateTime.UtcNow;
        }

        if (!string.IsNullOrEmpty(updateDto.Priority))
            issue.Priority = Enum.Parse<IssuePriority>(updateDto.Priority);

        issue.UpdatedAt = DateTime.UtcNow;
        var updatedIssue = await _issueReportRepository.UpdateAsync(issue);
        return await MapToIssueReportDtoAsync(updatedIssue);
    }

    public async Task<bool> DeleteIssueAsync(Guid issueId)
    {
        return await _issueReportRepository.DeleteAsync(issueId);
    }

    private async Task<IssueReportDto> MapToIssueReportDtoAsync(IssueReport issue)
    {
        var facility = await _facilityRepository.GetByIdAsync(issue.FacilityId);
        var createdBy = await _userRepository.GetByIdAsync(issue.CreatedById);
        var assignedTo = issue.AssignedToId.HasValue ? await _staffRepository.GetByIdAsync(issue.AssignedToId.Value) : null;

        return new IssueReportDto
        {
            IssueId = issue.IssueId,
            FacilityId = issue.FacilityId,
            CreatedById = issue.CreatedById,
            AssignedToId = issue.AssignedToId,
            FacilityType = facility?.FacilityType ?? "Unknown",
            CreatedByName = createdBy?.Name ?? "Unknown",
            AssignedToName = assignedTo?.Name,
            Description = issue.Description,
            Priority = issue.Priority.ToString(),
            Status = issue.Status.ToString(),
            CreatedAt = issue.CreatedAt
        };
    }
}
