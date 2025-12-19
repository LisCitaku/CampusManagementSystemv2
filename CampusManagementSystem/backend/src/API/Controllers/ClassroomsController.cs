using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ClassroomDto>> GetClassroomById(Guid id)
    {
        var classroom = await _classroomService.GetClassroomByIdAsync(id);
        if (classroom == null)
            return NotFound();
        return Ok(classroom);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ClassroomDto>>> GetAllClassrooms()
    {
        var classrooms = await _classroomService.GetAllClassroomsAsync();
        return Ok(classrooms);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClassroomDto>> CreateClassroom(CreateClassroomDto createClassroomDto)
    {
        try
        {
            var classroom = await _classroomService.CreateClassroomAsync(createClassroomDto);
            return CreatedAtAction(nameof(GetClassroomById), new { id = classroom.ClassroomId }, classroom);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClassroomDto>> UpdateClassroom(Guid id, UpdateClassroomDto updateClassroomDto)
    {
        try
        {
            var classroom = await _classroomService.UpdateClassroomAsync(id, updateClassroomDto);
            return Ok(classroom);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClassroom(Guid id)
    {
        var result = await _classroomService.DeleteClassroomAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
