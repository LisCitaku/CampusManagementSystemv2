using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Enums;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IStaffRepository _staffRepository;

    public ReservationService(IReservationRepository reservationRepository, IClassroomRepository classroomRepository, IStaffRepository staffRepository)
    {
        _reservationRepository = reservationRepository;
        _classroomRepository = classroomRepository;
        _staffRepository = staffRepository;
    }

    public async Task<ReservationDto?> GetReservationByIdAsync(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        return reservation == null ? null : await MapToReservationDtoAsync(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetStaffReservationsAsync(Guid staffId)
    {
        var reservations = await _reservationRepository.GetByStaffIdAsync(staffId);
        var result = new List<ReservationDto>();
        foreach (var reservation in reservations)
        {
            result.Add(await MapToReservationDtoAsync(reservation));
        }
        return result;
    }

    public async Task<IEnumerable<ReservationDto>> GetClassroomReservationsAsync(Guid classroomId)
    {
        var reservations = await _reservationRepository.GetByClassroomIdAsync(classroomId);
        var result = new List<ReservationDto>();
        foreach (var reservation in reservations)
        {
            result.Add(await MapToReservationDtoAsync(reservation));
        }
        return result;
    }

    public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto createReservationDto)
    {
        // Check for overlapping reservations
        var overlapping = await _reservationRepository.GetOverlappingReservationsAsync(
            createReservationDto.ClassroomId,
            createReservationDto.StartTime,
            createReservationDto.EndTime);

        if (overlapping.Any())
            throw new InvalidOperationException("Classroom is already reserved for this time slot");

        var classroom = await _classroomRepository.GetByIdAsync(createReservationDto.ClassroomId);
        if (classroom == null)
            throw new InvalidOperationException("Classroom not found");

        var staff = await _staffRepository.GetByIdAsync(createReservationDto.StaffId);
        if (staff == null)
            throw new InvalidOperationException("Staff not found");

        var reservation = new Reservation
        {
            ReservationId = Guid.NewGuid(),
            ClassroomId = createReservationDto.ClassroomId,
            StaffId = createReservationDto.StaffId,
            StartTime = createReservationDto.StartTime,
            EndTime = createReservationDto.EndTime,
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var createdReservation = await _reservationRepository.AddAsync(reservation);
        return await MapToReservationDtoAsync(createdReservation);
    }

    public async Task<ReservationDto> UpdateReservationAsync(Guid reservationId, UpdateReservationDto updateReservationDto)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            throw new InvalidOperationException("Reservation not found");

        if (updateReservationDto.StartTime.HasValue)
            reservation.StartTime = updateReservationDto.StartTime.Value;

        if (updateReservationDto.EndTime.HasValue)
            reservation.EndTime = updateReservationDto.EndTime.Value;

        if (!string.IsNullOrEmpty(updateReservationDto.Status))
            reservation.Status = Enum.Parse<ReservationStatus>(updateReservationDto.Status);

        reservation.UpdatedAt = DateTime.UtcNow;
        var updatedReservation = await _reservationRepository.UpdateAsync(reservation);
        return await MapToReservationDtoAsync(updatedReservation);
    }

    public async Task<bool> DeleteReservationAsync(Guid reservationId)
    {
        return await _reservationRepository.DeleteAsync(reservationId);
    }

    private async Task<ReservationDto> MapToReservationDtoAsync(Reservation reservation)
    {
        var classroom = await _classroomRepository.GetByIdAsync(reservation.ClassroomId);
        var staff = await _staffRepository.GetByIdAsync(reservation.StaffId);

        return new ReservationDto
        {
            ReservationId = reservation.ReservationId,
            ClassroomId = reservation.ClassroomId,
            StaffId = reservation.StaffId,
            ClassroomType = classroom?.Type ?? "Unknown",
            StaffName = staff?.Name ?? "Unknown",
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Status = reservation.Status.ToString()
        };
    }
}
