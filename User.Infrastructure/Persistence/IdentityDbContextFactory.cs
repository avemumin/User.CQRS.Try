using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace User.Infrastructure.Persistence;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
  public IdentityDbContext CreateDbContext(string[] args)
  {
    var cfg = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json")
      .Build();

    var connectionString = cfg.GetConnectionString("PostDb");

    var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
   
    return new IdentityDbContext(optionsBuilder.Options);
  }
}
