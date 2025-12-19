using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IssueReportsController : ControllerBase
{
    private readonly IIssueReportService _issueReportService;

    public IssueReportsController(IIssueReportService issueReportService)
    {
        _issueReportService = issueReportService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IssueReportDto>> GetIssueById(Guid id)
    {
        var issue = await _issueReportService.GetIssueByIdAsync(id);
        if (issue == null)
            return NotFound();
        return Ok(issue);
    }

    [HttpGet("facility/{facilityId}")]
    public async Task<ActionResult<IEnumerable<IssueReportDto>>> GetFacilityIssues(Guid facilityId)
    {
        var issues = await _issueReportService.GetFacilityIssuesAsync(facilityId);
        return Ok(issues);
    }

    [HttpGet("open")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<IEnumerable<IssueReportDto>>> GetOpenIssues()
    {
        var issues = await _issueReportService.GetOpenIssuesAsync();
        return Ok(issues);
    }

    [HttpGet("assigned/{staffId}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<ActionResult<IEnumerable<IssueReportDto>>> GetAssignedToStaff(Guid staffId)
    {
        var issues = await _issueReportService.GetAssignedToStaffAsync(staffId);
        return Ok(issues);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<IssueReportDto>> CreateIssue(CreateIssueReportDto createIssueDto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            var issue = await _issueReportService.CreateIssueAsync(createIssueDto, userId);
            return CreatedAtAction(nameof(GetIssueById), new { id = issue.IssueId }, issue);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<IssueReportDto>> UpdateIssue(Guid id, UpdateIssueReportDto updateIssueDto)
    {
        try
        {
            var issue = await _issueReportService.UpdateIssueAsync(id, updateIssueDto);
            return Ok(issue);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteIssue(Guid id)
    {
        var result = await _issueReportService.DeleteIssueAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
