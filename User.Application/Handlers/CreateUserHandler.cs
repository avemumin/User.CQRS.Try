using User.Application.Commands;
using User.Application.DTOs;
using User.Infrastructure.Interfaces;

namespace User.Application.Handlers;

public class CreateUserHandler
{
  private readonly IUserRepository _repo;
  public CreateUserHandler(IUserRepository repo) => _repo = repo;

  public async Task<UserDto> Handle(CreateUser cmd)
  {
    var user = new Domain.Entities.User(cmd.Name, cmd.LastName, cmd.Email, cmd.Age);
    await _repo.AddAsync(user);
    return new UserDto(user.Id, user.Name, user.LastName, user.Email, user.Age);
  }
}
