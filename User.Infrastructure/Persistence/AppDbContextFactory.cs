using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace User.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder.UseSqlServer("Server=.;Database=UserCQRS;User Id=sa;Password=5432!qaz;TrustServerCertificate=True;");

    return new AppDbContext(optionsBuilder.Options);
  }
}
