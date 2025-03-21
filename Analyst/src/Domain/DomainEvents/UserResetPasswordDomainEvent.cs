using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.DomainEvents;

public record UserResetPasswordDomainEvent(Guid UserId) : IDomainEvent;
