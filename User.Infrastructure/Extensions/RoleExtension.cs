using User.Infrastructure.Helpers;

namespace User.Infrastructure.Extensions;
public static class RoleExtension
{
  public static string ToRoleName(this AppRole role) => role.ToString();
}
