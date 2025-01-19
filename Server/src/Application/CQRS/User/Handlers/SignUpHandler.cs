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
using Application.Abstractions.Messaging;


namespace Application.CQRS.User.Handlers;

public class SignUpHandler(
    IUserService userService,
    IMapper mapper,
    ILogger<SignUpHandler> logger)
    : ICommandHandler<SignUpCommand, AppUserViewModel>
{
    public async Task<Result<AppUserViewModel>> Handle(
        SignUpCommand request,
         CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);
        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

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

