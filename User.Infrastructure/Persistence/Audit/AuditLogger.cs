using User.Application.Common.Interfaces;
using User.Application.DTOs;
using User.Infrastructure.Persistence.Entities;

namespace User.Infrastructure.Persistence.Audit;

public class AuditLogger : IAuditLogger
{
  private readonly AppDbContext _context;

  public AuditLogger(AppDbContext context)
  {
    _context = context;
  }

  public async Task LogAsync(AuditEntryDto entry)
  {
    var log = new AuditLog
    {
      Id = Guid.NewGuid(),
      UserId = entry.UserId,
      Action = entry.Action,
      EntityType = entry.EntityType,
      EntityId = entry.EntityId,
      TraceId = entry.TraceId,
      Timestamp = DateTime.UtcNow,
      Details = entry.Details
    };
    _context.AuditLogs.Add(log);
    await _context.SaveChangesAsync();
  }
}
