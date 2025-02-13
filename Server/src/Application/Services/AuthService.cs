using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specification;


namespace Application.Services;

public class AuthService(
    IJwtProvider jwtProvider,
    ITokenService tokenService,
    IUserService userService,
    IUnitOfWork unitOfWork)
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
        
        await unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenResponse(accessToken, refreshToken);
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    public async Task<TokenResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var spec = new ValidRefreshTokenSpecification(refreshToken);

        var storedToken = await unitOfWork.Repository<RefreshToken>()
            .FindOneAsync(spec, cancellationToken);

        if (storedToken == null)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var user = await userService.GetUserByIdAsync(storedToken.UserId, cancellationToken);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }
        
        string newAccessToken = jwtProvider.Generate(user);
        string newRefreshToken = tokenService.GenerateRefreshToken();
        
        DateTime expiryDate = tokenService.GetRefreshTokenExpiryDate();
        DateTime? slidingExpiry = tokenService.GetSlidingExpirationDate();
        
        storedToken.Replace(newRefreshToken, expiryDate);
        storedToken.Revoke();
        await unitOfWork.Repository<RefreshToken>().SoftUpdateAsync(storedToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new TokenResponse(newAccessToken, newRefreshToken);
    }
 
}



