using User.Application.Commands;
using User.Application.Common.Interfaces;


namespace User.Application.Handlers;

public class DeleteUserHandler
{
  private readonly IUserRepository _repo;
  private readonly IAuditLogger _audit;
  private readonly IAuditBuilder _auditBuilder;
  public DeleteUserHandler(IUserRepository repo, IAuditLogger audit, IAuditBuilder auditBuilder)
  {
    _repo = repo;
    _audit = audit;
    _auditBuilder = auditBuilder;
  }

  public async Task Handle(DeleteUser cmd)
  {
    await _repo.DeleteAsync(cmd.Id);
    var auditEntry =  _auditBuilder
      .BuildAudit("TestUser", "DeleteUser","User",cmd.Id.ToString(), $"Deleted user with ID {cmd.Id}");
   
    await _audit.LogAsync(auditEntry);
  }
}
