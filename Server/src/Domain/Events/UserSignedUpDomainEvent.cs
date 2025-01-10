using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Events;

public record UserSignedUpDomainEvent(User User) : IDomainEvent;
