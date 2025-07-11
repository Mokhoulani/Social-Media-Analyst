namespace Application.Common.Interfaces;


public interface ITokenService
{
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiryDate();
    DateTime? GetSlidingExpirationDate();
}