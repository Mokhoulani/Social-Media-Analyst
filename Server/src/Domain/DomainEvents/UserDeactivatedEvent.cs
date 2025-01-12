using Domain.Interfaces;

namespace Domain.Events;

public record UserDeactivatedEvent(Guid UserId) : IDomainEvent;
