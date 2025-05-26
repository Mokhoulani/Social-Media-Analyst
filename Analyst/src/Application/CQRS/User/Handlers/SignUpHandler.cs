using Application.CQRS.User.Commands;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Domain.ValueObjects;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Domain.Shared.Extensions;
using Domain.Interfaces;

namespace Application.CQRS.User.Handlers;

public class SignUpHandler(
    IUserService userService,
    IPermissionService permissionService,
    IMapper mapper,
    ILogger<SignUpHandler> logger,
    IAuthService authService)
    : ICommandHandler<SignUpCommand, TokenResponseViewModel>
{
    public async Task<Result<TokenResponseViewModel>> Handle(
        SignUpCommand request,
         CancellationToken cancellationToken)
    {
        var result = Email.Create(request.Email)
            .Bind(email => Password.Create(request.Password)
                .Bind(password => FirstName.Create(request.FirstName)
                    .Bind(firstName => LastName.Create(request.LastName)
                        .Map(lastName => new
                        {
                            Email = email,
                            Password = password,
                            FirstName = firstName,
                            LastName = lastName
                        }))));
     
        if (result.IsFailure) return result.Error;

        var role = await permissionService.GetRoleByNameAsync("registered", cancellationToken);

        var user = Domain.Entities.User.Create(Guid.NewGuid(), result.Value.Email, result.Value.FirstName,
            result.Value.LastName, result.Value.Password);

        if (role.IsSuccess)
            user.Roles.Add(role.Value);

        var userResult = await userService.AddUserAsync(user, cancellationToken);

        if (userResult.IsFailure) return userResult.Error;

        var tokenResponse = await authService.GenerateTokenResponse(user, cancellationToken);

        if (tokenResponse.IsFailure)
        {
            logger.LogError("Failed to generate token response for user with ID: {UserId}", user.Id);
            return tokenResponse.Error;
        }

        logger.LogInformation("User with ID: {UserId} has been signed up successfully", user.Id);

        return Result.Success(new TokenResponseViewModel(
            tokenResponse.Value.AccessToken,
             tokenResponse.Value.RefreshToken));
    }
}