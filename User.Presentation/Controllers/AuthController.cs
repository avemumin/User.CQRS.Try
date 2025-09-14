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
      await _authService.RegisterAsync(registerDto);
      return Ok(new
      {
        message = "Rejestracja zakończona. Sprawdź maila i potwierdź konto."
      });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
      var token = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
      return Ok(new { token });
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
      var confirm = await _authService.ConfirmEmail(userId, token);
      return Ok(new { confirm });
    }

    [HttpOptions("api/auth/login")]
    public IActionResult Options()
    {
      return Ok();
    }
  }
}
