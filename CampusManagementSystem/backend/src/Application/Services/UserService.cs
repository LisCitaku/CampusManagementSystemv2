using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;

namespace CampusManagementSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user == null ? null : MapToUserDto(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToUserDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToUserDto).ToList();
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            PasswordHash = _authService.HashPassword(createUserDto.Password),
            RoleType = Enum.Parse<Domain.Enums.RoleType>(createUserDto.RoleType),
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);
        return MapToUserDto(createdUser);
    }

    public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!string.IsNullOrEmpty(updateUserDto.Name))
            user.Name = updateUserDto.Name;

        if (!string.IsNullOrEmpty(updateUserDto.Email))
        {
            var existingUser = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingUser != null && existingUser.UserId != userId)
                throw new InvalidOperationException("Email already in use");
            user.Email = updateUserDto.Email;
        }

        if (!string.IsNullOrEmpty(updateUserDto.Status))
            user.Status = Enum.Parse<Domain.Enums.UserStatus>(updateUserDto.Status);

        user.UpdatedAt = DateTime.UtcNow;
        var updatedUser = await _userRepository.UpdateAsync(user);
        return MapToUserDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        return await _userRepository.DeleteAsync(userId);
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            RoleType = user.RoleType.ToString(),
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt
        };
    }
}
