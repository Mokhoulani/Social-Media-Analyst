using Api.Abstractions;
using Application.CQRS.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;


[Route("api/[controller]")]

public class AuthController(ISender sender) : ApiController(sender)
{
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command, CancellationToken token)
    {
        var tokenResponse = await Sender.Send(command,token);
        return Ok(tokenResponse);
    }
    
    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestResetPassword(
        [FromBody] RequestPasswordResetCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
       
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }
}

