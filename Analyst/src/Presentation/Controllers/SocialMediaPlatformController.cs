using Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Domain.Errors;
using Application.CQRS.Platform.Queries;

namespace Presentation.Controllers;

[Route("api/[controller]")]
public class SocialMediaPlatformController(ISender sender) : ApiController(sender)
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllSocialMediaPlatformAysnc(
           CancellationToken cancellationToken)
    {
        var query = new GetSocialMediaPlatformQuery();

        var userResult = await Sender.Send(query, cancellationToken);

        return userResult.IsSuccess ? Ok(userResult.Value) : HandleFailure(userResult);
    }

}
