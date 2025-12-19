using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IClassroomService
{
    Task<ClassroomDto?> GetClassroomByIdAsync(Guid classroomId);
    Task<IEnumerable<ClassroomDto>> GetAllClassroomsAsync();
    Task<ClassroomDto> CreateClassroomAsync(CreateClassroomDto createClassroomDto);
    Task<ClassroomDto> UpdateClassroomAsync(Guid classroomId, UpdateClassroomDto updateClassroomDto);
    Task<bool> DeleteClassroomAsync(Guid classroomId);
}
