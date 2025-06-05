using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.DomainEvents;

namespace Application.CQRS.User.Events;

public class UserNotificationDomainEventHandler(
    IFirebaseNotificationService firebaseService,
    IUserDeviceService userDeviceService) :
    IDomainEventHandler<UserNotificationDomainEvent>
{
    public async Task Handle(
        UserNotificationDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var deviceToken = await userDeviceService.GetDeviceTokenByUserIdAsync(
            notification.UserId,
            cancellationToken);

        if (deviceToken.IsFailure)
            return;

        await firebaseService.SendAsync(
            "New Notification",
            notification.Message,
            deviceToken.Value.DeviceToken);
    }
}