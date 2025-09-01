using Microsoft.Extensions.Logging;
using User.Application.Commands;
using User.Application.Common.Extension;
using User.Application.Common.Interfaces;


namespace User.Application.Handlers;

public class DeleteUserHandler
{
  private readonly IUserRepository _repo;
  private readonly IAuditLogger _audit;
  private readonly IAuditBuilder _auditBuilder;
  private readonly ILogger<DeleteUserHandler> _logger;
  public DeleteUserHandler(IUserRepository repo, IAuditLogger audit, IAuditBuilder auditBuilder, ILogger<DeleteUserHandler> logger)
  {
    _repo = repo;
    _audit = audit;
    _auditBuilder = auditBuilder;
    _logger = logger;
  }

  public async Task Handle(DeleteUser cmd)
  {
    _logger.LogInformation("Start to DELETE user with id = {userId}.", cmd.Id);

    await _repo.DeleteAsync(cmd.Id);

    _logger.LogInformation("User {userId} successfully DELETED.", cmd.Id);

    var auditEntry = _auditBuilder
      .BuildAudit("TestUser", cmd.GetAuditActionName(), "User", cmd.Id.ToString(), $"Deleted user with ID {cmd.Id}");

    await _audit.LogAsync(auditEntry);
  }
}
