using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface ICourseService
{
    Task<CourseDto?> GetCourseByIdAsync(Guid courseId);
    Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
    Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto);
    Task<CourseDto> UpdateCourseAsync(Guid courseId, UpdateCourseDto updateCourseDto);
    Task<bool> DeleteCourseAsync(Guid courseId);
}
