using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Enums;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository, IStudentRepository studentRepository, ICourseRepository courseRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(Guid enrollmentId)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
        return enrollment == null ? null : await MapToEnrollmentDtoAsync(enrollment);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(Guid studentId)
    {
        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);
        var result = new List<EnrollmentDto>();
        foreach (var enrollment in enrollments)
        {
            result.Add(await MapToEnrollmentDtoAsync(enrollment));
        }
        return result;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId)
    {
        var enrollments = await _enrollmentRepository.GetByCourseIdAsync(courseId);
        var result = new List<EnrollmentDto>();
        foreach (var enrollment in enrollments)
        {
            result.Add(await MapToEnrollmentDtoAsync(enrollment));
        }
        return result;
    }

    public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto)
    {
        var existingEnrollment = await _enrollmentRepository.GetByStudentAndCourseAsync(createEnrollmentDto.StudentId, createEnrollmentDto.CourseId);
        if (existingEnrollment != null)
            throw new InvalidOperationException("Student is already enrolled in this course");

        var student = await _studentRepository.GetByIdAsync(createEnrollmentDto.StudentId);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        var course = await _courseRepository.GetByIdAsync(createEnrollmentDto.CourseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        var enrollment = new Enrollment
        {
            EnrollmentId = Guid.NewGuid(),
            StudentId = createEnrollmentDto.StudentId,
            CourseId = createEnrollmentDto.CourseId,
            Status = EnrollmentStatus.Pending,
            Timestamp = DateTime.UtcNow
        };

        var createdEnrollment = await _enrollmentRepository.AddAsync(enrollment);
        return await MapToEnrollmentDtoAsync(createdEnrollment);
    }

    public async Task<EnrollmentDto> UpdateEnrollmentStatusAsync(Guid enrollmentId, UpdateEnrollmentStatusDto updateDto)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null)
            throw new InvalidOperationException("Enrollment not found");

        enrollment.Status = Enum.Parse<EnrollmentStatus>(updateDto.Status);
        if (updateDto.Status == "Completed")
            enrollment.CompletedAt = DateTime.UtcNow;

        var updatedEnrollment = await _enrollmentRepository.UpdateAsync(enrollment);
        return await MapToEnrollmentDtoAsync(updatedEnrollment);
    }

    public async Task<bool> DeleteEnrollmentAsync(Guid enrollmentId)
    {
        return await _enrollmentRepository.DeleteAsync(enrollmentId);
    }

    private async Task<EnrollmentDto> MapToEnrollmentDtoAsync(Enrollment enrollment)
    {
        var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
        var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);

        return new EnrollmentDto
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            StudentName = student?.Name ?? "Unknown",
            CourseTitle = course?.Title ?? "Unknown",
            Status = enrollment.Status.ToString(),
            Timestamp = enrollment.Timestamp
        };
    }
}
