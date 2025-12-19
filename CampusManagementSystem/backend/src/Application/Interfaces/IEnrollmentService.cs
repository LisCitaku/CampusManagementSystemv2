using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentDto?> GetEnrollmentByIdAsync(Guid enrollmentId);
    Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(Guid studentId);
    Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId);
    Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto);
    Task<EnrollmentDto> UpdateEnrollmentStatusAsync(Guid enrollmentId, UpdateEnrollmentStatusDto updateDto);
    Task<bool> DeleteEnrollmentAsync(Guid enrollmentId);
}
