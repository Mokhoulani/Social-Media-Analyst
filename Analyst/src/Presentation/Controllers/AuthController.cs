using Presentation.Abstractions;
using Application.CQRS.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using presentation.Contracts.Auth;
using Microsoft.AspNetCore.Http;

namespace Presentation.Controllers;

[Route("api/[controller]")]
public class AuthController(ISender sender) : ApiController(sender)
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
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

internal class TokenResponse
{
}