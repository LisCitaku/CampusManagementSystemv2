using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FacilitiesController : ControllerBase
{
    private readonly IFacilityService _facilityService;

    public FacilitiesController(IFacilityService facilityService)
    {
        _facilityService = facilityService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<FacilityDto>> GetFacilityById(Guid id)
    {
        var facility = await _facilityService.GetFacilityByIdAsync(id);
        if (facility == null)
            return NotFound();
        return Ok(facility);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<FacilityDto>>> GetAllFacilities()
    {
        var facilities = await _facilityService.GetAllFacilitiesAsync();
        return Ok(facilities);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FacilityDto>> CreateFacility(CreateFacilityDto createFacilityDto)
    {
        try
        {
            var facility = await _facilityService.CreateFacilityAsync(createFacilityDto);
            return CreatedAtAction(nameof(GetFacilityById), new { id = facility.FacilityId }, facility);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FacilityDto>> UpdateFacility(Guid id, UpdateFacilityDto updateFacilityDto)
    {
        try
        {
            var facility = await _facilityService.UpdateFacilityAsync(id, updateFacilityDto);
            return Ok(facility);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFacility(Guid id)
    {
        var result = await _facilityService.DeleteFacilityAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
