using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class StaffRepository : GenericRepository<Staff>, IStaffRepository
{
    public StaffRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Staff?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.Reservations)
            .Include(s => s.AssignedIssues)
            .FirstOrDefaultAsync(s => s.UserId == id);
    }
}
