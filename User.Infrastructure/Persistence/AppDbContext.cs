using Microsoft.EntityFrameworkCore;
using User.Infrastructure.Persistence.Configuration;
using User.Infrastructure.Persistence.Entities;
namespace User.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
  public DbSet<Domain.Entities.User> Users => Set<Domain.Entities.User>();
  public DbSet<AuditLog> AuditLogs { get; set; }
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
    //modelBuilder.Entity<Domain.Entities.User>(entity =>
    //{
    //  entity.HasKey(u => u.Id);
    //  entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
    //  entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
    //  entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
    //  entity.Property(u => u.Age).IsRequired();
    //});

    //modelBuilder.Entity<AuditLog>(entity =>
    //{
    //  entity.ToTable("AuditLogs");

    //  entity.HasKey(e => e.Id);

    //  entity.Property(e => e.UserId)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //  entity.Property(e => e.Action)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //  entity.Property(e => e.EntityType)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //  entity.Property(e => e.EntityId)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //  entity.Property(e => e.TraceId)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //  entity.Property(e => e.Timestamp)
    //        .IsRequired();

    //  entity.Property(e => e.Details)
    //        .HasColumnType("nvarchar(max)");
    //});
  }
}
