using Microsoft.AspNetCore.Mvc;
using User.Application.Commands;
using User.Application.DTOs;
using User.Application.Queries;
using Wolverine;

namespace User.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly IMessageBus _bus;
  public UsersController(IMessageBus bus) => _bus = bus;

  [HttpGet]
  public Task<List<UserDto>> GetAll()
      => _bus.InvokeAsync<List<UserDto>>(new GetAllUsers());

  [HttpGet("{id}")]
  public Task<UserDto> GetById(int id)
      => _bus.InvokeAsync<UserDto>(new GetUserById(id));

  [HttpPost]
  public async Task Create([FromBody] CreateUser cmd)
      => await _bus.SendAsync(cmd);

  [HttpPut("{id}")]
  public async Task Update(int id, [FromBody] UpdateUser cmd)
      => await _bus.SendAsync(cmd with { Id = id });

  [HttpDelete("{id}")]
  public async Task Delete(int id)
      => await _bus.SendAsync(new DeleteUser(id));
}
