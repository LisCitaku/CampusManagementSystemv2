using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IReservationService
{
    Task<ReservationDto?> GetReservationByIdAsync(Guid reservationId);
    Task<IEnumerable<ReservationDto>> GetStaffReservationsAsync(Guid staffId);
    Task<IEnumerable<ReservationDto>> GetClassroomReservationsAsync(Guid classroomId);
    Task<ReservationDto> CreateReservationAsync(CreateReservationDto createReservationDto);
    Task<ReservationDto> UpdateReservationAsync(Guid reservationId, UpdateReservationDto updateReservationDto);
    Task<bool> DeleteReservationAsync(Guid reservationId);
}
