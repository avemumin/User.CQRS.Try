using System.Diagnostics;
using User.Application.Common.Interfaces;
using User.Application.DTOs;

namespace User.Application.Common.Helpers;

public class AuditBuilder : IAuditBuilder
{
  public AuditEntryDto BuildAudit(string userId, string action, string entityType, string entityId , string details)
  {
    var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
    return new AuditEntryDto(userId, action, entityType, entityId, traceId, details);
  }
}
