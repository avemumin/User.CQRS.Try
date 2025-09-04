using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Infrastructure.Persistence.Entities;

namespace User.Infrastructure.Persistence.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
  public void Configure(EntityTypeBuilder<AuditLog> builder)
  {
    builder.ToTable("AuditLogs");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.UserId)
          .IsRequired()
          .HasMaxLength(100);

    builder.Property(e => e.Action)
          .IsRequired()
          .HasMaxLength(100);

    builder.Property(e => e.EntityType)
          .IsRequired()
          .HasMaxLength(100);

    builder.Property(e => e.EntityId)
          .IsRequired()
          .HasMaxLength(100);

    builder.Property(e => e.TraceId)
          .IsRequired()
          .HasMaxLength(100);

    builder.Property(e => e.Timestamp)
          .IsRequired();

    builder.Property(e => e.Details)
          .HasColumnType("nvarchar(max)");
  }
}
