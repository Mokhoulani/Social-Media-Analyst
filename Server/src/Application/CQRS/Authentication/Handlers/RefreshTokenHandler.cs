using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.CQRS.Authentication.Commands;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.CQRS.Authentication.Handlers;


public class RefreshTokenHandler(IAuthService authService,
    HybridCache cache)
    : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(
        RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        string cacheKey = $"refresh-token-{command.RefreshToken}"; 
        
        var cachedToken = await cache.GetOrCreateAsync<TokenResponse>(
            cacheKey, async token =>
        {
            var storedToken = await authService.RefreshAsync(
                command.RefreshToken,cancellationToken);
            
            return storedToken;
        }, cancellationToken: cancellationToken);

        return cachedToken; 
    }
}