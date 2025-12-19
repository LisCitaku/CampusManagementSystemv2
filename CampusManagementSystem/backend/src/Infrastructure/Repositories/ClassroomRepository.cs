using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class ClassroomRepository : GenericRepository<Classroom>, IClassroomRepository
{
    public ClassroomRepository(ApplicationDbContext context) : base(context)
    {
    }
}
