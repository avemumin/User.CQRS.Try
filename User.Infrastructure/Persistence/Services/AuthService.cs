using System.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using User.Infrastructure.Extensions;
using User.Infrastructure.Helpers;
using User.Infrastructure.Persistence.Entities;
using User.Infrastructure.Persistence.Services.DTOs;

namespace User.Infrastructure.Persistence.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IdentityDbContext _identityDbContext;
  private readonly ITokenService _tokenService;
  private readonly IEmailSender _emailSender;
  private readonly IAppSettingsService _appSettingsService;
  private readonly ITurnstileValidatorService _turnstileValidator;
  public AuthService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IdentityDbContext identityDbContext,
    ITokenService tokenService,
    IEmailSender emailSender,
    IAppSettingsService appSettingsService,
    ITurnstileValidatorService turnstileValidator)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _tokenService = tokenService;
    _emailSender = emailSender;
    _identityDbContext = identityDbContext;
    _appSettingsService = appSettingsService;
    _turnstileValidator = turnstileValidator;
  }

  public async Task RegisterAsync(RegisterDto registerDto)
  {

    var isHuman = await _turnstileValidator.ValidateHumanAsync(registerDto.CaptchaToken);
    if (!isHuman)
      throw new SecurityException("CAPTCHA validation failed.");

    using var transaction = await _identityDbContext.Database.BeginTransactionAsync();
    try
    {
      var user = new ApplicationUser { UserName = registerDto.Name, Email = registerDto.Email };
      var result = await _userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded)
        throw new Exception("Rejestracja się nie powiodła");

      //assing default role 
      await _userManager.AddToRoleAsync(user, registerDto.DefaultRole);

      var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

      var confirmLink = _appSettingsService.GetConfirmationLink(user.Id, token);

      await _emailSender.SendEmailAsync(registerDto.Email, "Potwierdź konto", $"Kliknij w link: {confirmLink}");

      await transaction.CommitAsync();
    }
    catch
    {
      await transaction.RollbackAsync();
      throw;
    }
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

  public async Task<string> ConfirmEmail(string userId, string token)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
      return "Użytkownik nie istnieje.";

    if (user.EmailConfirmed)
      return "Konto zostało już wcześniej potwierdzone";

    var result = await _userManager.ConfirmEmailAsync(user, token);
    if (!result.Succeeded) return "Nieprawidłowy lub wygasły token.";

    await _userManager.RemoveFromRoleAsync(user, RoleExtension.ToRoleName(AppRole.Unregistred));
    await _userManager.AddToRoleAsync(user, RoleExtension.ToRoleName(AppRole.Confirmed));

    return "Email został potwierdzony.";
  }
}
