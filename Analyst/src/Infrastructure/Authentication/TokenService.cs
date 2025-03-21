using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Application.Common.Interfaces;

namespace Infrastructure.Authentication;

public class TokenService(IOptions<RefreshTokenOptions> refreshTokenOptions) : ITokenService
{
    private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public DateTime GetRefreshTokenExpiryDate()
    {
        return DateTime.UtcNow.AddDays(_refreshTokenOptions.ExpiryDays);
    }

    public DateTime? GetSlidingExpirationDate()
    {
        if (_refreshTokenOptions.EnableSlidingExpiration)
        {
            return DateTime.UtcNow.AddDays(_refreshTokenOptions.SlidingExpirationDays);
        }
        return null;
    }
}