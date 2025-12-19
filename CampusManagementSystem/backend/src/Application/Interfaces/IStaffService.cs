using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IStaffService
{
    Task<StaffDto?> GetStaffByIdAsync(Guid staffId);
    Task<IEnumerable<StaffDto>> GetAllStaffAsync();
    Task<StaffDto> CreateStaffAsync(CreateStaffDto createStaffDto);
    Task<StaffDto> UpdateStaffAsync(Guid staffId, UpdateStaffDto updateStaffDto);
    Task<bool> DeleteStaffAsync(Guid staffId);
}
