using User.Infrastructure.Helpers;

namespace User.Infrastructure.Persistence.Services;

public interface IAppSettingsService
{
  string GetConfirmationLink(string userId, string token);
  EmailConfiguration GetEmailConfiguration();
  string GetDbConnectionParams();
  (string, string) CapthaParams();
}
