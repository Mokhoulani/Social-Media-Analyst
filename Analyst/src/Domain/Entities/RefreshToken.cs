using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities;
public class RefreshToken : Entity<Guid> ,IAggregateRoot
{
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public Guid UserId { get; private init; }
    public User? User { get; private init; }

    private RefreshToken()
    {
    }

    public RefreshToken(Guid Id, Guid userId, string token, DateTime expiresAt): base(Id)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt)
    {
        return new RefreshToken(Guid.NewGuid(),userId, token, expiresAt);
    }

    /// <summary>
    /// Replaces the current token with a new one and updates the expiration time.
    /// </summary>
    public void Replace(string newToken, DateTime newExpiry)
    {
        Token = newToken;
        ExpiresAt = newExpiry;
    }
}