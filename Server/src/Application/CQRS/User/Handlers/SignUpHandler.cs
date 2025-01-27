using Application.Common.Modoles.ViewModels;
using Application.CQRS.User.Commands;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Domain.ValueObjects;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;


namespace Application.CQRS.User.Handlers;

public class SignUpHandler(
    IUserService userService,
    IMapper mapper,
    ILogger<SignUpHandler> logger)
    : ICommandHandler<SignUpCommand, AppUserViewModel>
{
    public async Task<AppUserViewModel> Handle(
        SignUpCommand request,
         CancellationToken cancellationToken)
    {
        Email emailResult = Email.Create(request.Email);
        FirstName firstNameResult = FirstName.Create(request.FirstName);
        LastName lastNameResult = LastName.Create(request.LastName);
        Password passwordResult = Password.Create(request.Password);

        var user = Domain.Entities.User.Create(
            Guid.NewGuid(),
            emailResult,
            firstNameResult,
            lastNameResult,
            passwordResult
        );

        await userService.AddUserAsync(user, cancellationToken);

        logger.LogInformation("User with ID: {UserId}  has been signed up successfully",
            user.Id);
        return mapper.Map<AppUserViewModel>(user);
    }
   
}

