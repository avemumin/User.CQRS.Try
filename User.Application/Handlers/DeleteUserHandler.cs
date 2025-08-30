using User.Application.Commands;
using User.Infrastructure.Interfaces;

namespace User.Application.Handlers;

public class DeleteUserHandler
{
  private readonly IUserRepository _repo;
  public DeleteUserHandler(IUserRepository repo) => _repo = repo;

  public async Task Handle(DeleteUser cmd)
  {
    await _repo.DeleteAsync(cmd.Id);
  }
}
