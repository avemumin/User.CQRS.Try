using System.Diagnostics;
using User.Application.Commands;
using User.Application.Common.Interfaces;
using User.Application.DTOs;


namespace User.Application.Handlers;

public class DeleteUserHandler
{
  private readonly IUserRepository _repo;
  private readonly IAuditLogger _audit;
  
  public DeleteUserHandler(IUserRepository repo, IAuditLogger audit)
  {
    _repo = repo;
    _audit = audit;
  }

  public async Task Handle(DeleteUser cmd)
  {
    var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString(); 
    await _repo.DeleteAsync(cmd.Id);
    var auditEntry = new AuditEntryDto
    {
      UserId = "TestUser",
      Action = "DeleteUser",
      EntityType = "User",
      EntityId = cmd.Id.ToString(),
      TraceId = traceId,//Guid.NewGuid().ToString(),
      Details = $"Deleted user with ID {cmd.Id}"
    };
    await _audit.LogAsync(auditEntry);
  }
}
