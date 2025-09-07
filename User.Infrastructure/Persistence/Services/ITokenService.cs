using User.Infrastructure.Persistence.Entities;

namespace User.Infrastructure.Persistence.Services;

public interface ITokenService
{
  string GenerateToken(ApplicationUser appUser);
}
