using User.Application.DTOs;

namespace User.Application.Common.Interfaces;

public interface IAuditBuilder
{
  AuditEntryDto BuildAudit(string userId, string action, string entityType, string entityId,string details);
}
