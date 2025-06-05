using MapsterMapper;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Domain.Errors;
using Domain.Shared;
using Application.CQRS.Usage.Queries;


namespace Application.CQRS.Usage.Handlers;

public sealed class GetUserSocialMediaUsageByUserIdHandler(
    IUserSocialMediaUsageService userSocialMediaUsageService,
    IMapper mapper)
    : IQueryHandler<GetUserSocialMediaUsageByUserIdQuery, List<UserSocialMediaUsageViewModel>>
{
    public async Task<Result<List<UserSocialMediaUsageViewModel>>> Handle(
        GetUserSocialMediaUsageByUserIdQuery command,
     CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(command.UserId, out var userId))
        {
            return Result.Failure<List<UserSocialMediaUsageViewModel>>(
                DomainErrors.UserUsageGoal.NotFound);
        }

        var usagesResult = await userSocialMediaUsageService.GetByUserIdAsync(
            userId,
            cancellationToken);

        if (usagesResult.IsFailure)
        {
            return Result.Failure<List<UserSocialMediaUsageViewModel>>(
                DomainErrors.UserUsageGoal.NotFound);
        }

        var viewModels = mapper.Map<List<UserSocialMediaUsageViewModel>>(usagesResult.Value);

        return Result.Success(viewModels);
    }

}


