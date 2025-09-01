namespace User.Application.DTOs;

public class AuditEntryDto
{
  public string UserId { get; set; }
  public string Action { get; set; }
  public string EntityType { get; set; }
  public string EntityId { get; set; }
  public string TraceId { get; set; }
  public string? Details { get; set; }
}
