using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using User.Infrastructure.Helpers;

namespace User.Infrastructure.Persistence.Services;

public class AppSettingsService : IAppSettingsService
{
  private readonly IConfiguration _configuration;
  private readonly EmailConfiguration _emailConfiguration;

  public AppSettingsService(IConfiguration configuration, IOptions<EmailConfiguration> emailConfiguration)
  {
    _configuration = configuration;
    _emailConfiguration = emailConfiguration.Value;
  }

  public (string,string) CapthaParams()
  {
    var siteK = _configuration["Turnstile:SiteKey"];
    var secretK = _configuration["Turnstile:SecretKey"];

    return (siteK!, secretK!);
  }

  public string GetConfirmationLink(string userId, string token)
  {
    var baseUrl = _configuration["Confirmation:BaseUrl"];
    var endpoint = _configuration["Confirmation:Endpoint"];
    return $"{baseUrl}{endpoint}?userId={userId}&token={Uri.EscapeDataString(token)}";
  }

  public string GetDbConnectionParams()
  {
    var dbSettings = _configuration["ConnectionStrings:PostDb"];
    return dbSettings!;
  }

  public EmailConfiguration GetEmailConfiguration()
  => _emailConfiguration;
}
