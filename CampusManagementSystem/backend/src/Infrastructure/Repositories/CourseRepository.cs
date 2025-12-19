using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Course>> GetCoursesWithEnrollmentsAsync()
    {
        return await _dbSet.Include(c => c.Enrollments).ToListAsync();
    }
}
