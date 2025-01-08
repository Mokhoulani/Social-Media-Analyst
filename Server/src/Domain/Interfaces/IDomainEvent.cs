namespace Domain.Interfaces;


/// <summary>
/// Represents a domain event with an identifier and a timestamp.
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime Timestamp { get; }
}

