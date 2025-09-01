using User.Application.DTOs;

namespace User.Application.Common.Interfaces;
public interface IAuditLogger
{
  Task LogAsync(AuditEntryDto entry);
}