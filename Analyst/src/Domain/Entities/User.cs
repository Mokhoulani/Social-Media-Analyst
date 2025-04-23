using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;
public sealed class User : AggregateRoot<Guid>, IAggregateRoot
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

    public IEnumerable<RefreshToken>? RefreshTokens { get; private set; }
    public IEnumerable<PasswordResetToken>? PasswordResetTokens { get; private set; }
    public IEnumerable<Role> Roles { get; private set; }

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