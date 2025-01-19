using Api.Abstractions;
using Application.Common.Modoles.ViewModels;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Queries;
using Domain.Exceptions;
using Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/[controller]")]
public class UserController : ApiController
{
    public UserController(ISender sender)
        : base(sender)
    {
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="command">The CreateUserCommand</param>
    /// <returns>The ID of the newly created user</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(AppUserViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser(
        [FromBody] SignUpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetUserById), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Get a user by their ID
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <returns>The user details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppUserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email);

        Result<string> tokenResult = await Sender.Send(
            command,
            cancellationToken);

        if (tokenResult.IsFailure)
        {
            return HandleFailure(tokenResult);
        }

        return Ok(tokenResult.Value);
    }
}