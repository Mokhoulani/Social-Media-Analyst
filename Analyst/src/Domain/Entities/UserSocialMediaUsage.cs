using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class UserSocialMediaUsage : AggregateRoot<int>,
        IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public int PlatformId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        public User? User { get; private set; }
        public SocialMediaPlatform? Platform { get; private set; }

        protected UserSocialMediaUsage()
        {
        }

        private UserSocialMediaUsage(
            int id,
            Guid userId,
            int platformId,
            DateTime startTime) : base(id)
        {
            UserId = userId;
            PlatformId = platformId;
            StartTime = startTime;
        }

        public static UserSocialMediaUsage Create(
            int id,
            Guid userId,
            int platformId,
            DateTime startTime)
        {
            return new UserSocialMediaUsage(
                id,
                userId,
                platformId,
                startTime);
        }

        public void EndUsage(DateTime endTime)
        {
            if (EndTime.HasValue)
                throw new InvalidOperationException("Usage has already ended.");

            if (endTime <= StartTime)
                throw new ArgumentException("End time must be after start time.");

            EndTime = endTime;

            RaiseDomainEvent(new UserSocialMediaUsageEndedDomainEvent(
                Id,
                UserId,
                PlatformId));
        }

    }
}