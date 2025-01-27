using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;
using Domain.ValueObjects;




namespace Domain.Entities;
public sealed class User : AggregateRoot, IAggregateRoot
{
    private User(Guid id, Email email, FirstName firstName, LastName lastName, Password password)
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }

    private User()
    {
    }

    public Email Email { get; set; }

    public FirstName FirstName { get; set; }

    public LastName LastName { get; set; }

    public Password Password { get; set; }

    public static User Create(
        Guid id,
        Email email,
        FirstName firstName,
        LastName lastName,
        Password password)
    {


        var user = new User(
            id,
            email,
            firstName,
            lastName,
            password);

        user.RaiseDomainEvent(new UserSignedUpDomainEvent(user.Id));

        return user;
    }

    public bool VerifyPassword(string plainTextPassword)
    {
        return Password.Verify(plainTextPassword);
    }
}
