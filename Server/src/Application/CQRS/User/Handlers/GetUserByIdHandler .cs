using Application.Common.Modoles.ViewModels;
using Domain.Interfaces;
using MediatR;
using Application.CQRS.User.Queries;
using Application.Interfaces;
using MapsterMapper;
using Application.Abstractions.Messaging;
using Domain.Shared;


namespace Application.CQRS.User.Handlers;

public class GetUserByIdQueryHandler(
    IUserService userService,
    IMapper mapper)
    : ICommandHandler<GetUserByIdQuery, AppUserViewModel>
{


    public async Task<Result<AppUserViewModel>> Handle(
        GetUserByIdQuery request,
         CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(request.Id, cancellationToken);

        if (user == null)
            throw new Exception("User not found");

        return mapper.Map<AppUserViewModel>(user);
    }
}

