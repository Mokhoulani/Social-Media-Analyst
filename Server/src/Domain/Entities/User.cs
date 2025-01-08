using Domain.Base;
using Domain.Exceptions;
using Domain.Events;
using Domain.ValueObjects;
using Domain.Interfaces;

namespace Domain.Entities;

public class User : BaseEntity, IAggregateRoot
{
    public Email Email { get; private set; }
    public string Name { get; private set; }
    public ValueObjects.TimeZone TimeZone { get; private set; }
    public DateTime RegistrationDate { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<AppUsage> _appUsages = new();
    public IReadOnlyCollection<AppUsage> AppUsages => _appUsages.AsReadOnly();

    public User(Email email, string name, ValueObjects.TimeZone timeZone)
    {
        Email = email;
        Name = name;
        TimeZone = timeZone;
        RegistrationDate = DateTime.UtcNow;
        IsActive = true;
        AddDomainEvent(new UserCreatedEvent(Id, email.Value, name));
    }

    public void UpdateProfile(string name, ValueObjects.TimeZone timeZone)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be empty");

        Name = name;
        TimeZone = timeZone;
        AddDomainEvent(new UserProfileUpdatedEvent(Id, name, timeZone.Value));
    }
}
