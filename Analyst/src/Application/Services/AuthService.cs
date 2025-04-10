using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification;

namespace Application.Services;

public class AuthService(
    IJwtProvider jwtProvider,
    ITokenService tokenService,
    IUserService userService,
    IUnitOfWork unitOfWork) : IAuthService
{
    /// <summary>
    /// Handles user login, generates an access token and a refresh token.
    /// </summary>
    public async Task<Result<TokenResponseViewModel>> LoginAsync(LoginCommand command,
        CancellationToken cancellationToken)
    {
        var userResult = await userService.LoginAsync(command, cancellationToken);

        if (userResult.IsFailure) return DomainErrors.User.NotFound;

        var user = userResult.Value;

        var accessToken = jwtProvider.Generate(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var expiryDate = tokenService.GetRefreshTokenExpiryDate();

        var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, expiryDate);

        await unitOfWork.Repository<RefreshToken, Guid>().AddAsync(refreshTokenEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new TokenResponseViewModel(accessToken, refreshToken));
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    public async Task<Result<TokenResponseViewModel>> RefreshAsync(string refreshToken,
        CancellationToken cancellationToken)
    {
        var spec = new ValidRefreshTokenSpecification(refreshToken);

        var storedToken = await unitOfWork.Repository<RefreshToken, Guid>().FindOneAsync(spec, cancellationToken);

        if (storedToken.IsFailure) return Result.Failure<TokenResponseViewModel>(DomainErrors.NotFound<RefreshToken>());

        var user = await userService.GetUserByIdAsync(storedToken.Value.UserId, cancellationToken);

        if (user.IsFailure) return Result.Failure<TokenResponseViewModel>(DomainErrors.User.NotFound);

        var newAccessToken = jwtProvider.Generate(user.Value);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        var expiryDate = tokenService.GetRefreshTokenExpiryDate();

        storedToken.Value.Replace(newRefreshToken, expiryDate);

        var updateResult = await unitOfWork.Repository<RefreshToken, Guid>()
            .SoftUpdateAsync(storedToken.Value, cancellationToken);
        if (updateResult.IsFailure)
            return Result.Failure<TokenResponseViewModel>(DomainErrors.NotFound<RefreshToken>());

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new TokenResponseViewModel(newAccessToken, newRefreshToken));
    }
}