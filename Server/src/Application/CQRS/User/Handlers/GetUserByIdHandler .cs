using Application.CQRS.User.Queries;
using MapsterMapper;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;


namespace Application.CQRS.User.Handlers;

public class GetUserByIdQueryHandler(
    IUserService userService,
    IMapper mapper)
    : ICommandHandler<GetUserByIdQuery, AppUserViewModel>
{
    public async Task<AppUserViewModel> Handle(
        GetUserByIdQuery request,
         CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(Guid.Parse(request.Id), cancellationToken);

        if (user == null)
            throw new Exception("User not found");

        return mapper.Map<AppUserViewModel>(user);
    }
}

