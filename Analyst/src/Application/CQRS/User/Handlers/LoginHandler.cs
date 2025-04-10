using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Shared;


namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(IAuthService authService)
    : ICommandHandler<LoginCommand, TokenResponseViewModel>
{
    public async Task<Result<TokenResponseViewModel>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        return await authService.LoginAsync(
            command,cancellationToken);
    }
}
