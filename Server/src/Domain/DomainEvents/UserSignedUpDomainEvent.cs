using Domain.Primitives;


namespace Domain.DomainEvents;

public record UserSignedUpDomainEvent(Guid UserId) : IDomainEvent;
