using Application.Common.Modoles.ViewModels;
using MediatR;
using Application.CQRS.User.Commands;
using Application.Interfaces;
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Logging;


namespace Application.CQRS.User.Handlers;

public class SignUpHandler(
    IUserService userService,
    IMapper mapper,
    ILogger<SignUpHandler> logger)
    : IRequestHandler<SignUpCommand, AppUserViewModel>
{
    public async Task<AppUserViewModel> Handle(
        SignUpCommand command, 
        CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            UserName = command.Username,
            Email = command.Email
        };
        
        await userService.AddUserAsync(user, cancellationToken);
        
        logger.LogInformation("User with ID: {UserId}  has been signed up successfully",
            user.Id);
        return mapper.Map<AppUserViewModel>(user);
    }
}

