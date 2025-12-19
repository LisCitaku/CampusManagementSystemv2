using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAuthService _authService;

    public StudentService(IStudentRepository studentRepository, IAuthService authService)
    {
        _studentRepository = studentRepository;
        _authService = authService;
    }

    public async Task<StudentDto?> GetStudentByIdAsync(Guid studentId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        return student == null ? null : MapToStudentDto(student);
    }

    public async Task<StudentDto?> GetStudentByNumberAsync(string studentNumber)
    {
        var student = await _studentRepository.GetByStudentNumberAsync(studentNumber);
        return student == null ? null : MapToStudentDto(student);
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToStudentDto).ToList();
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto)
    {
        var student = new Student
        {
            UserId = Guid.NewGuid(),
            Name = createStudentDto.Name,
            Email = createStudentDto.Email,
            PasswordHash = _authService.HashPassword(createStudentDto.Password),
            StudentNumber = createStudentDto.StudentNumber,
            YearOfStudy = createStudentDto.YearOfStudy,
            RoleType = Domain.Enums.RoleType.Student,
            CreatedAt = DateTime.UtcNow
        };

        var createdStudent = await _studentRepository.AddAsync(student);
        return MapToStudentDto(createdStudent);
    }

    public async Task<StudentDto> UpdateStudentAsync(Guid studentId, UpdateStudentDto updateStudentDto)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        if (!string.IsNullOrEmpty(updateStudentDto.Name))
            student.Name = updateStudentDto.Name;

        if (!string.IsNullOrEmpty(updateStudentDto.Email))
            student.Email = updateStudentDto.Email;

        if (updateStudentDto.YearOfStudy.HasValue)
            student.YearOfStudy = updateStudentDto.YearOfStudy.Value;

        if (!string.IsNullOrEmpty(updateStudentDto.Status))
            student.Status = Enum.Parse<Domain.Enums.UserStatus>(updateStudentDto.Status);

        student.UpdatedAt = DateTime.UtcNow;
        var updatedStudent = await _studentRepository.UpdateAsync(student);
        return MapToStudentDto(updatedStudent);
    }

    public async Task<bool> DeleteStudentAsync(Guid studentId)
    {
        return await _studentRepository.DeleteAsync(studentId);
    }

    private static StudentDto MapToStudentDto(Student student)
    {
        return new StudentDto
        {
            UserId = student.UserId,
            Name = student.Name,
            Email = student.Email,
            StudentNumber = student.StudentNumber,
            YearOfStudy = student.YearOfStudy,
            RoleType = student.RoleType.ToString(),
            Status = student.Status.ToString(),
            CreatedAt = student.CreatedAt
        };
    }
}
