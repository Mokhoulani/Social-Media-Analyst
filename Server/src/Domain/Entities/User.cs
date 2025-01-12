using Domain.DomainEvents;
using Domain.Events;
using Domain.Interfaces;
using Domain.Primitives;
using Domain.ValueObjects;




namespace Domain.Entities;
public sealed class User : AggregateRoot, IAggregateRoot
{
    private User(Guid id, Email email, FirstName firstName, LastName lastName)
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    private User()
    {
    }

    public Email Email { get; set; }

    public FirstName FirstName { get; set; }

    public LastName LastName { get; set; }

    public static User Create(
        Guid id,
        Email email,
        FirstName firstName,
        LastName lastName)
    {
        var user = new User(
            id,
            email,
            firstName,
            lastName);

        user.RaiseDomainEvent(new UserSignedUpDomainEvent(user.Id));

        return user;
    }
}
