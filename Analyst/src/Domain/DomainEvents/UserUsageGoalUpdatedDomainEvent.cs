using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserUsageGoalUpdatedDomainEvent(int Id, Guid UserId, int PlatformId, TimeSpan DailyLimit, bool IsActive)
    : IDomainEvent;