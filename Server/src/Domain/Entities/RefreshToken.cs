using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities;
public class RefreshToken : AggregateRoot ,IAggregateRoot
{
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    private DateTime? RevokedAt { get; set; }
    public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    
    public Guid UserId { get; private init; }
    public User User { get; private init; }

    private RefreshToken()
    {
    }

    public RefreshToken(Guid userId, string token, DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
    {
        return new RefreshToken(userId, token, expiresAt);
    }
    
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Replaces the current token with a new one and updates the expiration time.
    /// </summary>
    public void Replace(string newToken, DateTime newExpiry)
    {
        Token = newToken;
        ExpiresAt = newExpiry;
        RevokedAt = null; // Ensure token remains active
    }
}
