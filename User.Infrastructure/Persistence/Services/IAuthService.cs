using User.Infrastructure.Persistence.Services.DTOs;

namespace User.Infrastructure.Persistence.Services;

public interface IAuthService
{
  Task RegisterAsync(RegisterDto registerDto);
  Task<string> LoginAsync(string email, string password);
  Task<string> ConfirmEmail(string userId, string token);
}

