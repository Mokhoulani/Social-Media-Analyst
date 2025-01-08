using Domain.Common;

namespace Domain.Events;

public record EntityDeletedEvent(int EntityId, string EntityType) : BaseDomainEvent;

