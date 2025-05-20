using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.DomainEvents;
using Domain.Interfaces;
using System.Text.Json;

namespace Application.CQRS.User.Events;

public class UserNotificationDomainEventHandler(IFirebaseNotificationService firebaseService) : IDomainEventHandler<UserNotificationDomainEvent>
{
    public async Task Handle(UserNotificationDomainEvent notification, CancellationToken cancellationToken)
    {
        // Here you must get the device token for the user
        var deviceToken = await GetDeviceTokenByUserId(notification.UserId);
        if (string.IsNullOrEmpty(deviceToken))
            return;

        await firebaseService.SendAsync(deviceToken, "New Notification", notification.Message);
    }

    private Task<string?> GetDeviceTokenByUserId(Guid userId)
    {
        // TODO: Implement retrieval of the device token from your database or cache
        throw new NotImplementedException();
    }
}

