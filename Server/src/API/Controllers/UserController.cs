using Api.Abstractions;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Queries;
using Application.CQRS.User.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/[controller]")]
public class UserController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="command">The CreateUserCommand</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The ID of the newly created user</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(AppUserViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(
        [FromBody] SignUpCommand command,
        CancellationToken cancellationToken)
    {
            var user = await Sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetUserById),
                new { id = user.Id }, user);
    }

    /// <summary>
    /// Get a user by their ID
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The user details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppUserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await Sender.Send(query, cancellationToken);
        
        return Ok(user);
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email,request.Password);

        var token = await Sender.Send(
            command,
            cancellationToken);
        
        return Ok(token);
    }
}