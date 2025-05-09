using Presentation.Abstractions;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using presentation.Contracts.User;

namespace Presentation.Controllers;

[Route("api/[controller]")]
public class UserController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The ID of the newly created user</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(AppUserViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        var command = new SignUpCommand(request.FirstName, request.Email, request.LastName, request.Password);
        var user = await Sender.Send(command, cancellationToken);

        return user.IsSuccess ? Ok(user.Value) : HandleFailure(user);
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
        var userResult = await Sender.Send(query, cancellationToken);

        return userResult.IsSuccess ? Ok(userResult.Value) : HandleFailure(userResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var tokenResult = await Sender.Send(command, cancellationToken);

        return tokenResult.IsSuccess ? Ok(tokenResult.Value) : HandleFailure(tokenResult);
    }
}