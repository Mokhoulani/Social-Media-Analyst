using Domain.Primitives;

namespace Domain.Entities;

public class RefreshToken :Entity
{
    public string Token { get; private init; }
    public DateTime ExpiresAt { get; init; }
    private DateTime? RevokedAt { get; set; }
    
    public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    
    public Guid UserId { get;private init; }
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

    public static RefreshToken Create(Guid userId, string token, TimeSpan validityPeriod)
    {
        return new RefreshToken(userId, token, DateTime.Now.Add(validityPeriod));
    }
    
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}