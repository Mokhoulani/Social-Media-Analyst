using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.Usage.Queries
{
    public record GetUserSocialMediaUsageByUserIdQuery(
        string UserId)
        : IQuery<Result<List<UserSocialMediaUsageViewModel>>>;
}