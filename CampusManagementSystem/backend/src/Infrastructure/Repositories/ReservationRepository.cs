using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Reservation>> GetByStaffIdAsync(Guid staffId)
    {
        return await _dbSet
            .Where(r => r.StaffId == staffId)
            .Include(r => r.Classroom)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByClassroomIdAsync(Guid classroomId)
    {
        return await _dbSet
            .Where(r => r.ClassroomId == classroomId)
            .Include(r => r.Staff)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(Guid classroomId, DateTime startTime, DateTime endTime)
    {
        return await _dbSet
            .Where(r => r.ClassroomId == classroomId &&
                   r.StartTime < endTime &&
                   r.EndTime > startTime)
            .ToListAsync();
    }
}
