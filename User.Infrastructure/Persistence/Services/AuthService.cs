using Microsoft.AspNetCore.Identity;
using User.Application.Common.Interfaces;
using User.Infrastructure.Persistence.Entities;

namespace User.Infrastructure.Persistence.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<ApplicationUser> _userManager;

  public AuthService(UserManager<ApplicationUser> userManager)
  {
    _userManager = userManager;
  }

  public async Task RegisterAsync(string email, string password)
  {
    var user = new ApplicationUser { UserName = email, Email = email };
    var result = await _userManager.CreateAsync(user, password);

    if (!result.Succeeded)
      throw new Exception("Rejestracja się nie powiodła");

  }
}
