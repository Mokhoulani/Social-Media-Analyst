using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities;

public class PasswordResetToken : AggregateRoot ,IAggregateRoot
{
    public Guid UserId { get; private init; } 
    public string Token { get; private init; }
    public DateTime ExpiresAt { get; init; }
    public bool Used { get; set; } = false;
    

    private PasswordResetToken() { }

    private PasswordResetToken(Guid userId, string token, DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static PasswordResetToken Create(Guid userId, string token, DateTime expiresAt)
    {
        var passwordResetToken = new PasswordResetToken(
            userId,
            token, 
            expiresAt);
        
        passwordResetToken.RaiseDomainEvent(new UserResetPasswordDomainEvent(userId));
        
        return passwordResetToken;
    }

    public void MarkAsUsed()
    {
        if (Used) throw new InvalidOperationException("Token has already been used.");
        Used = true;
    }
    
}
