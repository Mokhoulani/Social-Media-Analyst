using Application.Common.Modoles.ViewModels;
using MediatR;
using Application.CQRS.User.Commands;
using Application.Interfaces;
using Domain.Shared;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Domain.ValueObjects;
using Domain.Interfaces;
using Domain.Errors;


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
        Result<Email> emailResult = Email.Create(command.Email);
        Result<FirstName> firstNameResult = FirstName.Create(command.FirstName);
        Result<LastName> lastNameResult = LastName.Create(command.LastName);

        var user = Domain.Entities.User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            firstNameResult.Value,
            lastNameResult.Value);

        await userService.AddUserAsync(user, cancellationToken);

        logger.LogInformation("User with ID: {UserId}  has been signed up successfully",
            user.Id);
        return mapper.Map<AppUserViewModel>(user);
    }
}

