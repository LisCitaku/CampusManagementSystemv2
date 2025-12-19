using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EnrollmentDto>> GetEnrollmentById(Guid id)
    {
        var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
        if (enrollment == null)
            return NotFound();
        return Ok(enrollment);
    }

    [HttpGet("student/{studentId}")]
    [Authorize(Roles = "Student,Admin")]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetStudentEnrollments(Guid studentId)
    {
        var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
        return Ok(enrollments);
    }

    [HttpGet("course/{courseId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetCourseEnrollments(Guid courseId)
    {
        var enrollments = await _enrollmentService.GetCourseEnrollmentsAsync(courseId);
        return Ok(enrollments);
    }

    [HttpPost]
    [Authorize(Roles = "Student,Admin")]
    public async Task<ActionResult<EnrollmentDto>> CreateEnrollment(CreateEnrollmentDto createEnrollmentDto)
    {
        try
        {
            var enrollment = await _enrollmentService.CreateEnrollmentAsync(createEnrollmentDto);
            return CreatedAtAction(nameof(GetEnrollmentById), new { id = enrollment.EnrollmentId }, enrollment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EnrollmentDto>> UpdateEnrollmentStatus(Guid id, UpdateEnrollmentStatusDto updateDto)
    {
        try
        {
            var enrollment = await _enrollmentService.UpdateEnrollmentStatusAsync(id, updateDto);
            return Ok(enrollment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEnrollment(Guid id)
    {
        var result = await _enrollmentService.DeleteEnrollmentAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
