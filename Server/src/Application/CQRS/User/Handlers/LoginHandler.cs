using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.CQRS.User.Commands;
using Domain.Shared;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(IAuthService authService,
    HybridCache cache)
    : ICommandHandler<LoginCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
                return await authService.LoginAsync(
                    command,cancellationToken);
    }
}
