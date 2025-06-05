using Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts.Usage;
using Application.CQRS.Usage.Queries;
using Application.CQRS.Usage.Commands;
using Application.Common.Mod.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Presentation.Controllers;

[Route("api/[controller]")]
public class UserSocialMediaUsageController(
    ISender sender) : ApiController(sender)
{
    
    [HttpPost("get-usages")]
    [ProducesResponseType(typeof(UserSocialMediaUsageViewModel[]),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public async Task<IActionResult> GetUserUsageGoalsByUserId(
        [FromBody] GetUserSocialMediaUsageByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var command =
            new GetUserSocialMediaUsageByUserIdQuery(request.UserId);

        var usageResult = await Sender.Send(command,
            cancellationToken);

        return usageResult.IsSuccess
            ? Ok(usageResult.Value)
            : HandleFailure(usageResult);
    }

    [HttpPost("create-or-update-usage")]
    [ProducesResponseType(typeof(UserSocialMediaUsageViewModel),
        StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),
        StatusCodes.Status500InternalServerError)]
    [Produces("application/json")]
    public async Task<IActionResult> CreateOrUpdateUserSocialMediaUsage(
        [FromBody] CreateOrUpdateUserUsageRequest request,
        CancellationToken cancellationToken)
    {
        var command =
            new CreateOrUpdateUserSocialMediaUsageCommand(
                request.Id,
                request.UserId,
                request.PlatformId,
                request.StartTime);

        var usageResult = await Sender.Send(command,
            cancellationToken);

        return usageResult.IsSuccess
            ? Ok(usageResult.Value)
            : HandleFailure(usageResult);
    }
}