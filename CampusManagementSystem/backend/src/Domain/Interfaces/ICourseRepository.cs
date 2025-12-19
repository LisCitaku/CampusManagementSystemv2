using CampusManagementSystem.Domain.Entities;

namespace CampusManagementSystem.Domain.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetCoursesWithEnrollmentsAsync();
}
