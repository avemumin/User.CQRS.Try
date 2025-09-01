using User.Application.Common.Interfaces;
using User.Application.DTOs;
using User.Application.Queries;


namespace User.Application.Handlers;

public class GetUserByIdHandler
{
  private readonly IUserRepository _repo;
  public GetUserByIdHandler(IUserRepository repo) => _repo = repo;

  public async Task<UserDto> Handle(GetUserById query)
  {
    var u = await _repo.GetByIdAsync(query.Id)
        ?? throw new Exception("User not found");

    return new UserDto(u.Id, u.Name, u.LastName, u.Email, u.Age);
  }
}