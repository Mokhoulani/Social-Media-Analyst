using Domain.Common;

namespace Domain.Events;

public record UserCreatedEvent(
    int UserId, 
    string Email,
    string Name) : BaseDomainEvent;
