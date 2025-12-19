using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IStudentService
{
    Task<StudentDto?> GetStudentByIdAsync(Guid studentId);
    Task<StudentDto?> GetStudentByNumberAsync(string studentNumber);
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto);
    Task<StudentDto> UpdateStudentAsync(Guid studentId, UpdateStudentDto updateStudentDto);
    Task<bool> DeleteStudentAsync(Guid studentId);
}
