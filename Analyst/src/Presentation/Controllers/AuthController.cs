using Presentation.Abstractions;
using Application.CQRS.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using presentation.Contracts.Auth;
namespace Presentation.Controllers;


[Route("api/[controller]")]

public class AuthController(ISender sender) : ApiController(sender)
{

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request, CancellationToken token)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var tokenResponseResult = await Sender.Send(command, token);

        if (tokenResponseResult.IsSuccess)
            return Ok(tokenResponseResult.Value);

        return HandleFailure(tokenResponseResult);
    }

    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestResetPassword(
        [FromBody] RequestPasswordResetCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleFailure(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleFailure(result);
    }
}

