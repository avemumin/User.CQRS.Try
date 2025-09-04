using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace User.Infrastructure.Persistence;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
  public IdentityDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
    optionsBuilder.UseSqlServer("Server=.;Database=UserCQRS;User Id=sa;Password=5432!qaz;TrustServerCertificate=True;");

    return new IdentityDbContext(optionsBuilder.Options);
  }
}
