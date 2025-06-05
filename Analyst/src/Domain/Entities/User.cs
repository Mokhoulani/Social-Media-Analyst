using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;
public sealed class User : AggregateRoot<Guid>, IAggregateRoot , IAuditableEntity
{
    private User(Guid id, Email email, FirstName firstName, LastName lastName, Password password) : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }

    private User()
    {
    }

    public Email Email { get; init; } 
    
    public FirstName FirstName { get; init; }

    public LastName LastName { get; init; }

    public Password Password { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public ICollection<RefreshToken>? RefreshTokens { get; private set; } = [];
    public ICollection<PasswordResetToken>? PasswordResetTokens { get; private set; } = [];
    public ICollection<Role> Roles { get; private set; } = [];
    public ICollection<UserSocialMediaUsage>? SocialMediaUsages { get; private set; } = [];
    public ICollection<UserDevice>? Devices { get; private set; } = [];
    public ICollection<Notification>? Notifications { get; private set; } = [];
    
    public static User Create(Guid id, Email email, FirstName firstName, LastName lastName, Password password)
    {
        var user = new User(id, email, firstName, lastName, password);

        user.RaiseDomainEvent(new UserSignedUpDomainEvent(user.Id));

        return user;
    }

    public bool VerifyPassword(string plainTextPassword)
    {
        return Password.Verify(plainTextPassword);
    }

    public void SetPassword(Password newPassword)
    {
        Password = newPassword;
    }
}