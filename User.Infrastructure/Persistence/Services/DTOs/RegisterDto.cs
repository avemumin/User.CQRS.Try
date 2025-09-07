using User.Infrastructure.Extensions;

namespace User.Infrastructure.Persistence.Services.DTOs;

public class RegisterDto
{
  public string Email { get; set; }
  public string Name { get; set; } = "JohnDoe";
  public string Password { get; set; }

  public string CaptchaToken { get; set; }

  public string DefaultRole => RoleExtension.ToRoleName(Helpers.AppRole.Unregistred);
}
