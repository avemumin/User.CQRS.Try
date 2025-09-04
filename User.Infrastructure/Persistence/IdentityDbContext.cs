using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Infrastructure.Persistence.Entities;

namespace User.Infrastructure.Persistence;

public class IdentityDbContext : IdentityDbContext<ApplicationUser>
{
  public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
  {

  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<ApplicationUser>().ToTable("AuthUsers");
    builder.Entity<IdentityRole>().ToTable("AuthRoles");
    builder.Entity<IdentityUserRole<string>>().ToTable("AuthUserRoles");
    builder.Entity<IdentityUserClaim<string>>().ToTable("AuthUserClaims");
    builder.Entity<IdentityUserLogin<string>>().ToTable("AuthUserLogins");
    builder.Entity<IdentityRoleClaim<string>>().ToTable("AuthRoleClaims");
    builder.Entity<IdentityUserToken<string>>().ToTable("AuthUserTokens");
  }
}
