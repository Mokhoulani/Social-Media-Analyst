using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.Common.Mod.ViewModels;
using Application.CQRS.Authentication.Commands;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authentication.Handlers;


public class RefreshTokenHandler(IAuthService authService, ILogger<RefreshTokenHandler> logger)
    : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing refresh token request...");

        var (accessToken, refreshToken) = await authService.RefreshAsync(request.RefreshToken, cancellationToken);

        logger.LogInformation("New refresh token generated successfully.");

        return new TokenResponse(accessToken, refreshToken);
    }
}