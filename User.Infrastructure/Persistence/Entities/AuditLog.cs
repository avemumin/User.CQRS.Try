namespace User.Infrastructure.Persistence.Entities;

public class AuditLog
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string UserId { get; set; } = "anonymous";
  public string Action { get; set; }
  public string EntityType { get; set; } = "User";
  public string EntityId { get; set; }
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  public string TraceId { get; set; }
  public string? Details { get; set; }
}
