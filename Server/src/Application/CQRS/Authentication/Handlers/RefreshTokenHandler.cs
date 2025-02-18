using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.CQRS.Authentication.Commands;
using Domain.Shared;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.CQRS.Authentication.Handlers;


public class RefreshTokenHandler(IAuthService authService,
    HybridCache cache)
    : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        RefreshTokenCommand command, CancellationToken cancellationToken)
    {
            return await authService.RefreshAsync(
                command.RefreshToken,cancellationToken);
    }
}