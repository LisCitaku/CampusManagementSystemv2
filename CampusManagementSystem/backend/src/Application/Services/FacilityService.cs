using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class FacilityService : IFacilityService
{
    private readonly IFacilityRepository _facilityRepository;

    public FacilityService(IFacilityRepository facilityRepository)
    {
        _facilityRepository = facilityRepository;
    }

    public async Task<FacilityDto?> GetFacilityByIdAsync(Guid facilityId)
    {
        var facility = await _facilityRepository.GetByIdAsync(facilityId);
        return facility == null ? null : MapToFacilityDto(facility);
    }

    public async Task<IEnumerable<FacilityDto>> GetAllFacilitiesAsync()
    {
        var facilities = await _facilityRepository.GetAllAsync();
        return facilities.Select(MapToFacilityDto).ToList();
    }

    public async Task<FacilityDto> CreateFacilityAsync(CreateFacilityDto createFacilityDto)
    {
        var facility = new Facility
        {
            FacilityId = Guid.NewGuid(),
            FacilityType = createFacilityDto.FacilityType,
            CreatedAt = DateTime.UtcNow
        };

        var createdFacility = await _facilityRepository.AddAsync(facility);
        return MapToFacilityDto(createdFacility);
    }

    public async Task<FacilityDto> UpdateFacilityAsync(Guid facilityId, UpdateFacilityDto updateFacilityDto)
    {
        var facility = await _facilityRepository.GetByIdAsync(facilityId);
        if (facility == null)
            throw new InvalidOperationException("Facility not found");

        if (!string.IsNullOrEmpty(updateFacilityDto.FacilityType))
            facility.FacilityType = updateFacilityDto.FacilityType;

        if (!string.IsNullOrEmpty(updateFacilityDto.Status))
            facility.Status = Enum.Parse<Domain.Enums.FacilityStatus>(updateFacilityDto.Status);

        facility.UpdatedAt = DateTime.UtcNow;
        var updatedFacility = await _facilityRepository.UpdateAsync(facility);
        return MapToFacilityDto(updatedFacility);
    }

    public async Task<bool> DeleteFacilityAsync(Guid facilityId)
    {
        return await _facilityRepository.DeleteAsync(facilityId);
    }

    private static FacilityDto MapToFacilityDto(Facility facility)
    {
        return new FacilityDto
        {
            FacilityId = facility.FacilityId,
            FacilityType = facility.FacilityType,
            Status = facility.Status.ToString()
        };
    }
}
