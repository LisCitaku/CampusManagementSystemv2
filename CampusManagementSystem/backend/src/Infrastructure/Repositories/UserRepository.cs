using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        return await _dbSet
            .Where(u => u.RoleType.ToString() == role)
            .ToListAsync();
    }
}
