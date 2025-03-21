using Application.CQRS.User.Queries;
using MapsterMapper;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Domain.Errors;
using Domain.Shared;
using ZiggyCreatures.Caching.Fusion;


namespace Application.CQRS.User.Handlers;

public class GetUserByIdQueryHandler(
    IUserService userService,
    IFusionCache cache,
    IMapper mapper)
    : IQueryHandler<GetUserByIdQuery, AppUserViewModel>
{
    public async Task<Result<AppUserViewModel>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = Guid.TryParse(request.Id, out var userId);
        
        if (!result)
            return DomainErrors.User.NotFound;
        
        var userResult=  await userService.GetUserByIdAsync(userId, cancellationToken);
        return userResult.IsFailure ? userResult.Error : 
            mapper.Map<AppUserViewModel>(userResult.Value);
    }
}
