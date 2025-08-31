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
  public async Task<ActionResult<List<UserDto>>> GetAll()
  {
    var users = await _bus.InvokeAsync<List<UserDto>>(new GetAllUsers());
    return Ok(users);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<UserDto>> GetById(int id)
  {
    var user = await _bus.InvokeAsync<UserDto>(new GetUserById(id));
    return user is null ? NotFound() : Ok(user);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateUser cmd)
  {
    await _bus.InvokeAsync(cmd);
    return Ok();
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] UpdateUser cmd)
  {
    await _bus.SendAsync(cmd with { Id = id });
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    await _bus.SendAsync(new DeleteUser(id));
    return NoContent();
  }
}
