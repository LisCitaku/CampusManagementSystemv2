using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByStudentNumberAsync(string studentNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include(s => s.Enrollments).FirstOrDefaultAsync(s => s.UserId == id);
    }
}
