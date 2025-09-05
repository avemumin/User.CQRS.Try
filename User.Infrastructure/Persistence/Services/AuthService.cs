using Microsoft.AspNetCore.Identity;
using User.Infrastructure.Persistence.Entities;

namespace User.Infrastructure.Persistence.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly ITokenService _tokenService;
  public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _tokenService = tokenService;
  }

  public async Task<string> RegisterAsync(string email, string password)
  {
    var user = new ApplicationUser { UserName = email, Email = email };
    var result = await _userManager.CreateAsync(user, password);

    if (!result.Succeeded)
      throw new Exception("Rejestracja się nie powiodła");

    return _tokenService.GenerateToken(user);
  }

  public async Task<string> LoginAsync(string email, string password)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user is null)
      throw new Exception("Nieprawidłowe dane logowania");

    var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
    if (!result.Succeeded)
      throw new Exception("Nieprawidłowe dane logowania");

    return _tokenService.GenerateToken(user);
  }
}
