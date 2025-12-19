using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(Guid id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();
        return Ok(student);
    }

    [HttpGet("number/{studentNumber}")]
    public async Task<ActionResult<StudentDto>> GetStudentByNumber(string studentNumber)
    {
        var student = await _studentService.GetStudentByNumberAsync(studentNumber);
        if (student == null)
            return NotFound();
        return Ok(student);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto createStudentDto)
    {
        try
        {
            var student = await _studentService.CreateStudentAsync(createStudentDto);
            return CreatedAtAction(nameof(GetStudentById), new { id = student.UserId }, student);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<ActionResult<StudentDto>> UpdateStudent(Guid id, UpdateStudentDto updateStudentDto)
    {
        try
        {
            var student = await _studentService.UpdateStudentAsync(id, updateStudentDto);
            return Ok(student);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        var result = await _studentService.DeleteStudentAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
