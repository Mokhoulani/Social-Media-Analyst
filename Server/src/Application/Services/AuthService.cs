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
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }
        
        string refreshToken = tokenService.GenerateRefreshToken();
        DateTime expiryDate = tokenService.GetRefreshTokenExpiryDate();
        
        var refreshTokenEntity = RefreshToken.Create(
            user.Id, refreshToken, expiryDate);
        
        await refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        return new TokenResponse(accessToken, refreshToken);
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    public async Task<TokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var storedToken = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
        if (storedToken == null || !storedToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }
        
        var user = await userRepository.GetByIdAsync(storedToken.UserId, cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }
        
        string newAccessToken = jwtProvider.Generate(user);
        string newRefreshToken = tokenService.GenerateRefreshToken();
        
        DateTime expiryDate = tokenService.GetRefreshTokenExpiryDate();
        DateTime? slidingExpiry = tokenService.GetSlidingExpirationDate();
        
        storedToken.Replace(newRefreshToken, expiryDate);
        await refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);

        return new TokenResponse(newAccessToken, newRefreshToken);
    }
 
}


