using Domain.Interfaces;

namespace Domain.DomainEvents;

public record UserDeactivatedEvent(Guid UserId) : IDomainEvent;
