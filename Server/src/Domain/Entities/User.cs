using Domain.Events;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace Domain.Entities;

public class User : IdentityUser<int>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public User()
    {
        RaiseDomainEvent(new UserSignedUpDomainEvent(this));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
