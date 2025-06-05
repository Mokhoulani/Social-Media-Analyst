using Application.Common.Mod.ViewModels;
using Application.CQRS.Goal.Commands;
using Application.CQRS.Goal.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.Contracts.Goal;

namespace Presentation.Controllers;

[Route("api/[controller]")]
public class UserUsageGoalController(
    ISender sender) : ApiController(sender)
{
    [HttpPost("get-goals")]
    [ProducesResponseType(typeof(UserUsageGoalViewModel[]),
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
        [FromBody] GetUserUsageGoalsByUserId request,
        CancellationToken cancellationToken)
    {
        var command =
            new GetUserUsageGoalsByUserIdQuery(request.UserId);

        var goalResult = await Sender.Send(command,
            cancellationToken);

        return goalResult.IsSuccess
            ? Ok(goalResult.Value)
            : HandleFailure(goalResult);
    }

    [HttpPost("create-or-update-goal")]
    [ProducesResponseType(typeof(UserUsageGoalViewModel),
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
    public async Task<IActionResult> CreateOrUpdateUserUsageGoal(
        [FromBody] CreateOrUpdateUserUsageGoalRequest request,
        CancellationToken cancellationToken)
    {
        var command =
            new CreateOrUpdateUserUsageGoalCommand(
                request.Id,
                request.UserId,
                request.PlatformId,
                request.DailyLimit);

        var goalResult = await Sender.Send(command,
            cancellationToken);

        return goalResult.IsSuccess
            ? Ok(goalResult.Value)
            : HandleFailure(goalResult);
    }
}