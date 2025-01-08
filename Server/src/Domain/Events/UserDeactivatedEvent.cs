using Domain.Common;

namespace Domain.Events;

public record UserDeactivatedEvent(int UserId) : BaseDomainEvent;
