using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StaffDto>> GetStaffById(Guid id)
    {
        var staff = await _staffService.GetStaffByIdAsync(id);
        if (staff == null)
            return NotFound();
        return Ok(staff);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<StaffDto>>> GetAllStaff()
    {
        var staff = await _staffService.GetAllStaffAsync();
        return Ok(staff);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StaffDto>> CreateStaff(CreateStaffDto createStaffDto)
    {
        try
        {
            var staff = await _staffService.CreateStaffAsync(createStaffDto);
            return CreatedAtAction(nameof(GetStaffById), new { id = staff.UserId }, staff);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult<StaffDto>> UpdateStaff(Guid id, UpdateStaffDto updateStaffDto)
    {
        try
        {
            var staff = await _staffService.UpdateStaffAsync(id, updateStaffDto);
            return Ok(staff);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStaff(Guid id)
    {
        var result = await _staffService.DeleteStaffAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
