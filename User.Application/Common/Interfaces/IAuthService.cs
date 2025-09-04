namespace User.Application.Common.Interfaces;

public interface IAuthService
{
  Task RegisterAsync(string email, string password);
}
