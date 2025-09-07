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
  private readonly ILogger<UsersController> _logger;
  public UsersController(IMessageBus bus, ILogger<UsersController> logger)
  {
    _bus = bus;
    _logger = logger;
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<ActionResult<List<UserDto>>> GetAll()
  {
    var users = await _bus.InvokeAsync<List<UserDto>>(new GetAllUsers());
    if (users is null || users.Count == 0)
      return NoContent();
    return Ok(users);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<UserDto>> GetById(int id)
  {
    var user = await _bus.InvokeAsync<UserDto>(new GetUserById(id));
    return user is null ? NotFound() : Ok(user);
  }

  [HttpPost]
  [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Create([FromBody] CreateUser cmd)
  {
    var createdUser = await _bus.InvokeAsync<UserDto>(cmd);
    return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Update(int id, [FromBody] UpdateUser cmd)
  {
    await _bus.SendAsync(cmd with { Id = id });
    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete(int id)
  {
    await _bus.SendAsync(new DeleteUser(id));
    return NoContent();
  }
}
