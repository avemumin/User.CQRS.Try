namespace User.Infrastructure.Persistence.Services;

public interface ITurnstileValidatorService
{
  Task<bool> ValidateHumanAsync(string token);

}
