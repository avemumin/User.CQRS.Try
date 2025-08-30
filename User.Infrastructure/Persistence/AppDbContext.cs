using Microsoft.EntityFrameworkCore;
namespace User.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
  public DbSet<Domain.Entities.User> Users => Set<Domain.Entities.User>();

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Domain.Entities.User>(entity =>
    {
      entity.HasKey(u => u.Id);
      entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
      entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
      entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
      entity.Property(u => u.Age).IsRequired();
    });
  }
}
