using User.Application.Commands;
using User.Infrastructure.Interfaces;

namespace User.Application.Handlers;

public class CreateUserHandler
{
  private readonly IUserRepository _repo;
  public CreateUserHandler(IUserRepository repo) => _repo = repo;

  public async Task Handle(CreateUser cmd)
  {
    var user = new Domain.Entities.User(cmd.Name, cmd.LastName, cmd.Email, cmd.Age);
    await _repo.AddAsync(user);
  }
}
