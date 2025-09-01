using User.Application.Commands;
using User.Application.Common.Interfaces;

namespace User.Application.Handlers;

public class UpdateUserHandler
{
  private readonly IUserRepository _repo;
  public UpdateUserHandler(IUserRepository repo) => _repo = repo;

  public async Task Handle(UpdateUser cmd)
  {
    var user = await _repo.GetByIdAsync(cmd.Id)
        ?? throw new Exception("User not found");

    user.Update(cmd.Name, cmd.LastName, cmd.Email, cmd.Age);
    await _repo.UpdateAsync(user);
  }
}