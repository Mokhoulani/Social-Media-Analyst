using Application.CQRS.User.Commands;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Domain.ValueObjects;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

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
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<AppUserViewModel>(emailResult.Error);

        var firstNameResult = FirstName.Create(request.FirstName);
        if (firstNameResult.IsFailure)
            return Result.Failure<AppUserViewModel>(firstNameResult.Error);

        var lastNameResult = LastName.Create(request.LastName);
        if (lastNameResult.IsFailure)
            return Result.Failure<AppUserViewModel>(lastNameResult.Error);

        var passwordResult = Password.Create(request.Password);
        if (passwordResult.IsFailure)
            return Result.Failure<AppUserViewModel>(passwordResult.Error);

        var user = Domain.Entities.User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            firstNameResult.Value,
            lastNameResult.Value,
            passwordResult.Value
        );

        var userResult = await userService.AddUserAsync(user, cancellationToken);
        if (passwordResult.IsFailure)
            return Result.Failure<AppUserViewModel>(userResult.Error);

        logger.LogInformation("User with ID: {UserId} has been signed up successfully", user.Id);

        var userViewModel = mapper.Map<AppUserViewModel>(user);
        return Result.Success(userViewModel);
    }
}