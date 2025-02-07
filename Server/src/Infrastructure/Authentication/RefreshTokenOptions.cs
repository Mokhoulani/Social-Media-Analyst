namespace Infrastructure.Authentication;

public class RefreshTokenOptions
{
    public int ExpiryDays { get; init; }
    public bool EnableSlidingExpiration { get; init; }
    public int SlidingExpirationDays { get; init; }
}