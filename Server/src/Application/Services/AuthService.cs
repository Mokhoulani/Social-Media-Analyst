using Application.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    ITokenService tokenService,
    IUserService userService,
    IRefreshTokenRepository refreshTokenRepository)
    : IAuthService
{
    /// <summary>
    /// Handles user login, generates an access token and a refresh token.
    /// </summary>
    public async Task<TokenResponse> LoginAsync(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userService.LoginAsync(command, cancellationToken);
        
        string accessToken = jwtProvider.Generate(user);
        string refreshToken = tokenService.GenerateRefreshToken();

        // Store refresh token in the database
        var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, TimeSpan.FromDays(7));
        await refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        return new TokenResponse(accessToken, refreshToken);
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    public async Task<TokenResponse> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var storedToken = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

        if (storedToken == null || !storedToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        var user = await userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        // Generate new access and refresh tokens
        string newAccessToken = jwtProvider.Generate(user);
        string newRefreshToken = tokenService.GenerateRefreshToken();

        // Revoke old token and store the new one
        await refreshTokenRepository.RevokeTokenAsync(storedToken, cancellationToken);
        var newTokenEntity = RefreshToken.Create(user.Id, newRefreshToken, TimeSpan.FromDays(7));
        await refreshTokenRepository.AddAsync(newTokenEntity, cancellationToken);

        return new TokenResponse(newAccessToken,newRefreshToken);
    }
}


