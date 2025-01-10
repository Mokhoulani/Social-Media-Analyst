using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.User.Handlers;

public class UserSignedUpEventHandler(ILogger<UserSignedUpEventHandler> logger)
    : INotificationHandler<UserSignedUpDomainEvent>
{
    public async Task Handle(
        UserSignedUpDomainEvent notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("User signed up with ID and Email ");
        await Task.CompletedTask;
    }
}



