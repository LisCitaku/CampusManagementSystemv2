using CampusManagementSystem.Domain.Entities;

namespace CampusManagementSystem.Domain.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetByStudentIdAsync(Guid studentId);
    Task<IEnumerable<Enrollment>> GetByCourseIdAsync(Guid courseId);
    Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId);
}
