using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid email or password");
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(CreateUserDto createUserDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(Register), new { id = user.UserId }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
