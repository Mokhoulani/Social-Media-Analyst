using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserSocialMediaUsageEndedDomainEvent(int Id, Guid UserId, int PlatformId) : IDomainEvent;