using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Authentication.Commands;
using Domain.Shared;


namespace Application.CQRS.Authentication.Handlers;


public class RefreshTokenHandler(IAuthService authService)
    : ICommandHandler<RefreshTokenCommand, TokenResponseViewModel>
{
    public async Task<Result<TokenResponseViewModel>> Handle(
        RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        return await authService.RefreshAsync(
            command.RefreshToken,cancellationToken);
    }
}