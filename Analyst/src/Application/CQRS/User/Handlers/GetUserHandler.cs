using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;
using MapsterMapper;

namespace Application.CQRS.User.Handlers;

internal sealed class GetUserHandler(IUserService userService, ICurrentUser currentUser, IMapper mapper)
    : ICommandHandler<GetUserCommand, AppUserViewModel>
{
    public async Task<Result<AppUserViewModel>> Handle(
        GetUserCommand command,
        CancellationToken cancellationToken)
    {
       
        var email = currentUser.Email;

        if (string.IsNullOrWhiteSpace(email))
        {
            return DomainErrors.User.NotFound;
        }

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        
        var userResult = await userService.GetByEmailAsync(emailResult.Value, cancellationToken);
        if (userResult.IsFailure)
        {
            return userResult.Error;
        }
    
        return mapper.Map<AppUserViewModel>(userResult.Value);
    }
}
