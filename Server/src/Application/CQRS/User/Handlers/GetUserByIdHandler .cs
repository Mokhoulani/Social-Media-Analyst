using Application.CQRS.User.Queries;
using MapsterMapper;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
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
        var userResult=  await userService.GetUserByIdAsync(Guid.Parse(request.Id), cancellationToken);
        return userResult.IsFailure ? userResult.Error : 
            mapper.Map<AppUserViewModel>(userResult.Value);
    }
}


// var cacheKey = $"User_{request.Id}"; 
//         
// var userResult = await cache.GetOrSetAsync<Result<Domain.Entities.User>>(
//     cacheKey,
//     async _ => await userService.GetUserByIdAsync(Guid.Parse(request.Id), cancellationToken),
//     options => options
//         .SetDuration(TimeSpan.FromMinutes(1))  
//         .SetFailSafe(true, TimeSpan.FromHours(1), TimeSpan.FromSeconds(30)),
//     token: cancellationToken 
// );

