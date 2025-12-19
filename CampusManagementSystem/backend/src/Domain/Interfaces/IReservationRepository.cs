using CampusManagementSystem.Domain.Entities;

namespace CampusManagementSystem.Domain.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetByStaffIdAsync(Guid staffId);
    Task<IEnumerable<Reservation>> GetByClassroomIdAsync(Guid classroomId);
    Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(Guid classroomId, DateTime startTime, DateTime endTime);
}
