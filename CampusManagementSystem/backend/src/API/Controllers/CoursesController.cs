using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CourseDto>> GetCourseById(Guid id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
            return NotFound();
        return Ok(course);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto createCourseDto)
    {
        try
        {
            var course = await _courseService.CreateCourseAsync(createCourseDto);
            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, course);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(Guid id, UpdateCourseDto updateCourseDto)
    {
        try
        {
            var course = await _courseService.UpdateCourseAsync(id, updateCourseDto);
            return Ok(course);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
