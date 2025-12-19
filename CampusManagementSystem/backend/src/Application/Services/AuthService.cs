using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CampusManagementSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _jwtExpirationMinutes;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _jwtSecret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        _jwtIssuer = configuration["Jwt:Issuer"] ?? "CampusManagementAPI";
        _jwtAudience = configuration["Jwt:Audience"] ?? "CampusManagementClient";
        _jwtExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null || !VerifyPasswordAsync(loginDto.Password, user.PasswordHash).Result)
            throw new UnauthorizedAccessException("Invalid email or password");

        var userDto = new UserDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            RoleType = user.RoleType.ToString(),
            Status = user.Status.ToString(),
            CreatedAt = user.CreatedAt
        };

        var (token, _) = await GenerateTokensAsync(userDto);

        return new LoginResponseDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            RoleType = user.RoleType.ToString(),
            Token = token
        };
    }

    public async Task<(string token, string refreshToken)> GenerateTokensAsync(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.RoleType)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Generate refresh token (simplified - in production use a proper implementation)
        var refreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.UserId}:{DateTime.UtcNow}"));

        return (tokenString, refreshToken);
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hash)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hash));
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
