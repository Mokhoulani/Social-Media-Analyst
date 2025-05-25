using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Application.CQRS.Platform.Queries
{
    [AllowAnonymous]
    public record GetSocialMediaPlatformQuery :
    IQuery<Result<List<SocialMediaPlatfomViewModel>>>;
}