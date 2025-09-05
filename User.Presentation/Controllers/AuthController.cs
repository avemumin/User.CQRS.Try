using Microsoft.AspNetCore.Mvc;
using User.Infrastructure.Persistence.Services;
using User.Infrastructure.Persistence.Services.DTOs;

namespace User.Presentation.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
      var token = await _authService.RegisterAsync(registerDto.Email, registerDto.Password);
      return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
      var token = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
      return Ok(new { token });
    }
  }
}
