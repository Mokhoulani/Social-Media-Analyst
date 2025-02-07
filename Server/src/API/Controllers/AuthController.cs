
using Api.Abstractions;
using Application.CQRS.Authentication.Commands;
using Application.CQRS.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;


[Route("api/[controller]")]

public class AuthController(ISender sender, ILogger<UserController> logger) : ApiController(sender, logger)
{
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command, CancellationToken token)
    {
        var tokenResponse = await Sender.Send(command,token);
        return Ok(tokenResponse);
    }
}



