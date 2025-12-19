using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IAuthService _authService;

    public StaffService(IStaffRepository staffRepository, IAuthService authService)
    {
        _staffRepository = staffRepository;
        _authService = authService;
    }

    public async Task<StaffDto?> GetStaffByIdAsync(Guid staffId)
    {
        var staff = await _staffRepository.GetByIdAsync(staffId);
        return staff == null ? null : MapToStaffDto(staff);
    }

    public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
    {
        var staff = await _staffRepository.GetAllAsync();
        return staff.Select(MapToStaffDto).ToList();
    }

    public async Task<StaffDto> CreateStaffAsync(CreateStaffDto createStaffDto)
    {
        var staff = new Staff
        {
            UserId = Guid.NewGuid(),
            Name = createStaffDto.Name,
            Email = createStaffDto.Email,
            PasswordHash = _authService.HashPassword(createStaffDto.Password),
            StaffType = createStaffDto.StaffType,
            RoleType = Domain.Enums.RoleType.Staff,
            CreatedAt = DateTime.UtcNow
        };

        var createdStaff = await _staffRepository.AddAsync(staff);
        return MapToStaffDto(createdStaff);
    }

    public async Task<StaffDto> UpdateStaffAsync(Guid staffId, UpdateStaffDto updateStaffDto)
    {
        var staff = await _staffRepository.GetByIdAsync(staffId);
        if (staff == null)
            throw new InvalidOperationException("Staff not found");

        if (!string.IsNullOrEmpty(updateStaffDto.Name))
            staff.Name = updateStaffDto.Name;

        if (!string.IsNullOrEmpty(updateStaffDto.Email))
            staff.Email = updateStaffDto.Email;

        if (!string.IsNullOrEmpty(updateStaffDto.StaffType))
            staff.StaffType = updateStaffDto.StaffType;

        if (!string.IsNullOrEmpty(updateStaffDto.Status))
            staff.Status = Enum.Parse<Domain.Enums.UserStatus>(updateStaffDto.Status);

        staff.UpdatedAt = DateTime.UtcNow;
        var updatedStaff = await _staffRepository.UpdateAsync(staff);
        return MapToStaffDto(updatedStaff);
    }

    public async Task<bool> DeleteStaffAsync(Guid staffId)
    {
        return await _staffRepository.DeleteAsync(staffId);
    }

    private static StaffDto MapToStaffDto(Staff staff)
    {
        return new StaffDto
        {
            UserId = staff.UserId,
            Name = staff.Name,
            Email = staff.Email,
            StaffType = staff.StaffType,
            RoleType = staff.RoleType.ToString(),
            Status = staff.Status.ToString(),
            CreatedAt = staff.CreatedAt
        };
    }
}
