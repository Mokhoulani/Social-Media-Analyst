using Domain.Primitives;

namespace Domain.DomainEvents;

public record UserResetPasswordDomainEvent(Guid UserId) : IDomainEvent;
