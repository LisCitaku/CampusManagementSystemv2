using CampusManagementSystem.Application.DTOs;

namespace CampusManagementSystem.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<(string token, string refreshToken)> GenerateTokensAsync(UserDto user);
    Task<bool> VerifyPasswordAsync(string password, string hash);
    string HashPassword(string password);
}
