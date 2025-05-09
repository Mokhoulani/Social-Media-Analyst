using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserNotificationDomainEvent(Guid UserId) : IDomainEvent;