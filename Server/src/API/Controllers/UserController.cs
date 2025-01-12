using Application.Common.Modoles.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Queries;
using Domain.Exceptions;
using FluentValidation;
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
    [HttpPost("signup")]
    [ProducesResponseType(typeof(AppUserViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] SignUpCommand command)
    {
        try
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ValidationProblemDetails(
                ex.Errors.GroupBy(x => x.PropertyName)
                    .ToDictionary(g =>
                        g.Key, g => 
                        g.Select(x => x.ErrorMessage).ToArray())
            ));
        }
        catch (DomainException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Domain Validation Error",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError
                });
        }
    }


    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <returns>The user data</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
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
