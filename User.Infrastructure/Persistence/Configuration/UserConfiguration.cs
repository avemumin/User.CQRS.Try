using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
{
  public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
  {
    builder.ToTable("Users");
    builder.HasKey(u => u.Id);
    builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
    builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
    builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
    builder.Property(u => u.Age).IsRequired();
    builder.Property(u => u.IdentityUserId)
      .IsRequired()
      .HasMaxLength(450);
    builder.HasIndex(u => u.IdentityUserId).IsUnique();
  }
}
