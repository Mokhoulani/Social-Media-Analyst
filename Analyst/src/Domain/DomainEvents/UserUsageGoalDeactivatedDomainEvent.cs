using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserUsageGoalDeactivatedDomainEvent(int Id, Guid UserId, int PlatformId) : IDomainEvent;