using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(
    IUserService userService,
    IJwtProvider jwtProvider,
    IAuthService authService)
    : ICommandHandler<LoginCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
         return await authService.LoginAsync(command, cancellationToken);
    }
}
