using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public CourseService(ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<CourseDto?> GetCourseByIdAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            return null;

        var enrollments = await _enrollmentRepository.GetByCourseIdAsync(courseId);
        return MapToCourseDto(course, enrollments.Count());
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetCoursesWithEnrollmentsAsync();
        return courses.Select(c => MapToCourseDto(c, c.Enrollments.Count)).ToList();
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        var course = new Course
        {
            CourseId = Guid.NewGuid(),
            Title = createCourseDto.Title,
            CreditPoints = createCourseDto.CreditPoints,
            MaxCapacity = createCourseDto.MaxCapacity,
            CreatedAt = DateTime.UtcNow
        };

        var createdCourse = await _courseRepository.AddAsync(course);
        return MapToCourseDto(createdCourse, 0);
    }

    public async Task<CourseDto> UpdateCourseAsync(Guid courseId, UpdateCourseDto updateCourseDto)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        if (!string.IsNullOrEmpty(updateCourseDto.Title))
            course.Title = updateCourseDto.Title;

        if (updateCourseDto.CreditPoints.HasValue)
            course.CreditPoints = updateCourseDto.CreditPoints.Value;

        if (updateCourseDto.MaxCapacity.HasValue)
            course.MaxCapacity = updateCourseDto.MaxCapacity.Value;

        course.UpdatedAt = DateTime.UtcNow;
        var updatedCourse = await _courseRepository.UpdateAsync(course);

        var enrollments = await _enrollmentRepository.GetByCourseIdAsync(courseId);
        return MapToCourseDto(updatedCourse, enrollments.Count());
    }

    public async Task<bool> DeleteCourseAsync(Guid courseId)
    {
        return await _courseRepository.DeleteAsync(courseId);
    }

    private static CourseDto MapToCourseDto(Course course, int enrollmentCount)
    {
        return new CourseDto
        {
            CourseId = course.CourseId,
            Title = course.Title,
            CreditPoints = course.CreditPoints,
            MaxCapacity = course.MaxCapacity,
            CurrentEnrollments = enrollmentCount
        };
    }
}
