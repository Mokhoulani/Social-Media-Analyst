using Application.Common.Interfaces;
using Application.Common.Mod;
using Application.CQRS.User.Commands;
using Domain.Common.Exceptions;
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
    IUnitOfWork unitOfWork)
    : IAuthService
{
    /// <summary>
    /// Handles user login, generates an access token and a refresh token.
    /// </summary>
    public async Task<Result<TokenResponse>> LoginAsync(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var userResult = await userService.LoginAsync(command, cancellationToken);

        if (userResult.IsFailure)
            return DomainErrors.User.NotFound;

        var user = userResult.Value;
    
        string accessToken = jwtProvider.Generate(user);
        string refreshToken = tokenService.GenerateRefreshToken();
        DateTime expiryDate = tokenService.GetRefreshTokenExpiryDate();
    
        var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, expiryDate);
    
        await unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new TokenResponse(accessToken, refreshToken));
    }


    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    public async Task<Result<TokenResponse>> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var spec = new ValidRefreshTokenSpecification(refreshToken);

        var storedToken = await unitOfWork.Repository<RefreshToken>()
            .FindOneAsync(spec, cancellationToken);

        if (storedToken.IsFailure)
            return Result.Failure<TokenResponse>(DomainErrors.NotFound<RefreshToken>());

        var user = await userService.GetUserByIdAsync(storedToken.Value.UserId, cancellationToken);
        
        if (user.IsFailure)
           return Result.Failure<TokenResponse>(DomainErrors.User.NotFound);
        
        string newAccessToken = jwtProvider.Generate(user.Value);
        string newRefreshToken = tokenService.GenerateRefreshToken();
        
        DateTime expiryDate = tokenService.GetRefreshTokenExpiryDate();
        
        storedToken.Value.Replace(newRefreshToken, expiryDate);
        
       var updateResult = await unitOfWork.Repository<RefreshToken>().SoftUpdateAsync(storedToken.Value, cancellationToken);
       if (updateResult.IsFailure)
           return Result.Failure<TokenResponse>(DomainErrors.NotFound<RefreshToken>());
       
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(new TokenResponse(newAccessToken, newRefreshToken));
    }
 
}



