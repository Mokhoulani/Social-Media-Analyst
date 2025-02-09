using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.CQRS.User.Commands;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.CQRS.User.Handlers;

internal sealed class LoginCommandHandler(IAuthService authService,
    HybridCache cache)
    : ICommandHandler<LoginCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        string cacheKey = $"refresh-token-{command}"; 
        
        var cachedToken = await cache.GetOrCreateAsync<TokenResponse>(
            cacheKey, async token =>
            {
                var storedToken = await authService.LoginAsync(
                    command,cancellationToken);
            
                return storedToken;
            }, cancellationToken: cancellationToken);

        return cachedToken;
    }
}
