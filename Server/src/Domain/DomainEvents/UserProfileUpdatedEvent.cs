using Domain.Interfaces;

namespace Domain.Events;

public record UserProfileUpdatedEvent(Guid UserId) : IDomainEvent;
