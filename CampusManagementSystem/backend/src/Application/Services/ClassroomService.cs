using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class ClassroomService : IClassroomService
{
    private readonly IClassroomRepository _classroomRepository;

    public ClassroomService(IClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task<ClassroomDto?> GetClassroomByIdAsync(Guid classroomId)
    {
        var classroom = await _classroomRepository.GetByIdAsync(classroomId);
        return classroom == null ? null : MapToClassroomDto(classroom);
    }

    public async Task<IEnumerable<ClassroomDto>> GetAllClassroomsAsync()
    {
        var classrooms = await _classroomRepository.GetAllAsync();
        return classrooms.Select(MapToClassroomDto).ToList();
    }

    public async Task<ClassroomDto> CreateClassroomAsync(CreateClassroomDto createClassroomDto)
    {
        var classroom = new Classroom
        {
            ClassroomId = Guid.NewGuid(),
            Type = createClassroomDto.Type,
            Capacity = createClassroomDto.Capacity,
            CreatedAt = DateTime.UtcNow
        };

        var createdClassroom = await _classroomRepository.AddAsync(classroom);
        return MapToClassroomDto(createdClassroom);
    }

    public async Task<ClassroomDto> UpdateClassroomAsync(Guid classroomId, UpdateClassroomDto updateClassroomDto)
    {
        var classroom = await _classroomRepository.GetByIdAsync(classroomId);
        if (classroom == null)
            throw new InvalidOperationException("Classroom not found");

        if (!string.IsNullOrEmpty(updateClassroomDto.Type))
            classroom.Type = updateClassroomDto.Type;

        if (updateClassroomDto.Capacity.HasValue)
            classroom.Capacity = updateClassroomDto.Capacity.Value;

        classroom.UpdatedAt = DateTime.UtcNow;
        var updatedClassroom = await _classroomRepository.UpdateAsync(classroom);
        return MapToClassroomDto(updatedClassroom);
    }

    public async Task<bool> DeleteClassroomAsync(Guid classroomId)
    {
        return await _classroomRepository.DeleteAsync(classroomId);
    }

    private static ClassroomDto MapToClassroomDto(Classroom classroom)
    {
        return new ClassroomDto
        {
            ClassroomId = classroom.ClassroomId,
            Type = classroom.Type,
            Capacity = classroom.Capacity
        };
    }
}
