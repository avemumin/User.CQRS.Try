using Microsoft.Extensions.Logging;
using User.Application.Commands;
using User.Application.Common.Interfaces;
using User.Application.DTOs;


namespace User.Application.Handlers;

public class CreateUserHandler
{
  private readonly IUserRepository _repo;
  private readonly ILogger<CreateUserHandler> _logger;
  public CreateUserHandler(IUserRepository repo, ILogger<CreateUserHandler> logger)
  {
    _repo = repo;
    _logger = logger;
  }

  public async Task<UserDto> Handle(CreateUser cmd)
  {
    _logger.LogInformation("Start to CREATE user with Email:{cmd.Email}.", cmd.Email);
    var user = new Domain.Entities.User(cmd.Name, cmd.LastName, cmd.Email, cmd.Age);
    await _repo.AddAsync(user);
    _logger.LogInformation("User {cmd.Email} CREATED.", cmd.Email);
    return new UserDto(user.Id, user.Name, user.LastName, user.Email, user.Age);
  }
}
