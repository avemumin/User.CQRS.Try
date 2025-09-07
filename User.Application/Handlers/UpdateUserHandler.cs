using User.Application.Commands;
using User.Application.Common.Extension;
using User.Application.Common.Interfaces;

namespace User.Application.Handlers;

public class UpdateUserHandler
{
  private readonly IUserRepository _repo;
  private readonly IAuditLogger _audit;
  private readonly IAuditBuilder _auditBuilder;
  public UpdateUserHandler(IUserRepository repo, IAuditLogger audit, IAuditBuilder auditBuilder)
  {
    _repo = repo;
    _audit = audit;
    _auditBuilder = auditBuilder;
  }

  public async Task Handle(UpdateUser cmd)
  {
    var user = await _repo.GetByIdAsync(cmd.Id)
        ?? throw new Exception("User not found");

    user.Update(cmd.Name, cmd.LastName, cmd.Email, cmd.Age);
    await _repo.UpdateAsync(user);
    var auditEntry = _auditBuilder
      .BuildAudit("TestUser", cmd.GetAuditActionName(), "User", cmd.Id.ToString(), $"Edited user with ID {cmd.Id}");

    await _audit.LogAsync(auditEntry);
  }
}