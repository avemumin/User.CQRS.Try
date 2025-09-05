using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using User.Infrastructure.Extensions;

namespace User.Infrastructure.Helpers;

public static class SeedRoles
{
  public static async Task SeedRolesAsync(IServiceProvider service)
  {
    var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (AppRole role  in Enum.GetValues(typeof(AppRole)))
    {
      var roleName = role.ToRoleName();
      if(!await roleManager.RoleExistsAsync(roleName))
      {
        await roleManager.CreateAsync(new IdentityRole(roleName));
      }
    }
  }
}
