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
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken token)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var tokenResponseResult = await Sender.Send(command, token);

        return tokenResponseResult.IsSuccess ? Ok(tokenResponseResult.Value) : HandleFailure(tokenResponseResult);
    }

    [HttpPost("request-reset")]
    public async Task<IActionResult> RequestResetPassword([FromBody] RequestPasswordResetRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RequestPasswordResetCommand(request.Email);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(request.Token, request.Password);
        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
}