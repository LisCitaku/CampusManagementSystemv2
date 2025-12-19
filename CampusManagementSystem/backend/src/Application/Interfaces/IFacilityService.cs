using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IFacilityService
{
    Task<FacilityDto?> GetFacilityByIdAsync(Guid facilityId);
    Task<IEnumerable<FacilityDto>> GetAllFacilitiesAsync();
    Task<FacilityDto> CreateFacilityAsync(CreateFacilityDto createFacilityDto);
    Task<FacilityDto> UpdateFacilityAsync(Guid facilityId, UpdateFacilityDto updateFacilityDto);
    Task<bool> DeleteFacilityAsync(Guid facilityId);
}
