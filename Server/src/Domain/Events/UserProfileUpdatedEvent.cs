using Domain.Common;

namespace Domain.Events;

public record UserProfileUpdatedEvent(
    int UserId,
    string NewName,
    string NewTimeZone) : BaseDomainEvent;
