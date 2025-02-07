using Domain.Interfaces;

namespace Domain.DomainEvents;

public record UserProfileUpdatedEvent(Guid UserId) : IDomainEvent;
