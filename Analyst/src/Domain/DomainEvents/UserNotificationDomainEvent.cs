using Domain.Enums;
using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserNotificationDomainEvent(
    Guid UserId,
    string Message,
    NotificationType Type
) : IDomainEvent;
