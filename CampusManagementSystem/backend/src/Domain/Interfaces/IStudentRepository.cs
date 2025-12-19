using CampusManagementSystem.Domain.Entities;

namespace CampusManagementSystem.Domain.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByStudentNumberAsync(string studentNumber);
}
