using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserUsageGoalActivatedDomainEvent(int Id, Guid UserId, int PlatformId) : IDomainEvent;