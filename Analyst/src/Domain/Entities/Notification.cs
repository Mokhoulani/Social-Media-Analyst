using Domain.DomainEvents;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Notification : AggregateRoot<Guid>,
        IAggregateRoot,
        IAuditableEntity
    {
        public Guid UserId { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public NotificationType Type { get; private set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }

        public User? User { get; private set; }

        protected Notification()
        {
        }

        public Notification(Guid id,
            Guid userId,
            string message,
            NotificationType type)
        {
            UserId = userId;
            Message = message;
            Type = type;
        }

        public static Notification Create(Guid userId,
            string message,
            NotificationType type)
        {
            var notification = new Notification(Guid.NewGuid(),
                userId,
                message,
                type);
            notification.RaiseDomainEvent(new UserNotificationDomainEvent(
                userId,
                message,
                type));
            return notification;
        }
    }
}