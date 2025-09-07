namespace User.Infrastructure.Persistence.Entities;

public class AuditLog
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string UserId { get; set; } = "anonymous";
  public string Action { get; set; } = "Action";
  public string EntityType { get; set; } = "User";
  public string EntityId { get; set; } = "EntityId";
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  public string TraceId { get; set; } = Guid.NewGuid().ToString();
  public string? Details { get; set; }
}
