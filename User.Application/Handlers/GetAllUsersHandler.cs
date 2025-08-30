using User.Application.DTOs;
using User.Application.Queries;
using User.Infrastructure.Interfaces;

namespace User.Application.Handlers;

public class GetAllUsersHandler
{
  private readonly IUserRepository _repo;
  public GetAllUsersHandler(IUserRepository repo) => _repo = repo;

  public async Task<List<UserDto>> Handle(GetAllUsers query)
  {
    var users = await _repo.GetAllAsync();
    return users.Select(u => new UserDto(u.Id, u.Name, u.LastName, u.Email, u.Age)).ToList();
  }
}