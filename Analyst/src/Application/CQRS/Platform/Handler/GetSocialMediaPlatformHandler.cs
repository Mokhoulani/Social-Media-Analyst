using MapsterMapper;
using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Application.CQRS.Platform.Queries;
using Application.Common.Interfaces;


namespace Application.CQRS.Goal.Handlers;

public class GetSocialMediaPlatformHandler(
    ISocialMediaPlatFormService service,
    IMapper mapper)
    : IQueryHandler<GetSocialMediaPlatformQuery, List<SocialMediaPlatfomViewModel>>
{
    public async Task<Result<List<SocialMediaPlatfomViewModel>>> Handle(
       GetSocialMediaPlatformQuery command,
       CancellationToken cancellationToken)
    {
        var platformsResult = await service.GetAllPlatforms(cancellationToken);

        if (platformsResult.IsFailure)
            return platformsResult.Error;

        return mapper.Map<List<SocialMediaPlatfomViewModel>>(platformsResult.Value);
    }
}


