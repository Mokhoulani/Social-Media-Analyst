using Microsoft.AspNetCore.Mvc;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Queries;
using MediatR;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="command">The CreateUserCommand</param>
    /// <returns>The ID of the newly created user</returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <returns>The user data</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query); // Use _mediator.Send to handle the query
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
