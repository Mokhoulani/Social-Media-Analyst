using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities;

public class PasswordResetToken : AggregateRoot<Guid> ,IAggregateRoot
{
    public string Token { get; private init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public bool Used { get; set; } = false;
    public Guid UserId { get; private init; }
    public User? User { get; private init; }

    private PasswordResetToken()
    {
    }

    private PasswordResetToken(Guid Id,Guid userId, string token, DateTime expiresAt) : base(Id)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static PasswordResetToken Create(Guid userId, string token, DateTime expiresAt)
    {
        var passwordResetToken = new PasswordResetToken(Guid.NewGuid(), userId, token, expiresAt);

        passwordResetToken.RaiseDomainEvent(new UserResetPasswordDomainEvent(userId));

        return passwordResetToken;
    }

    public void MarkAsUsed()
    {
        if (Used) throw new InvalidOperationException("Token has already been used.");
        Used = true;
    }
}