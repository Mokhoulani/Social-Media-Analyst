using Domain.DomainEvents;
using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class UserUsageGoal : AggregateRoot<int>,
        IAggregateRoot,
        IAuditableEntity
    {
        public Guid UserId { get; private set; }
        public int PlatformId { get; private set; }

        /// <summary>
        /// Maximum allowed usage duration (e.g., 1 hour per day).
        /// </summary>
        public TimeSpan DailyLimit { get; private set; }

        /// <summary>
        /// Whether the goal is currently active.
        /// </summary>
        public bool IsActive { get; private set; } = true;

        /// <summary>
        /// Optional notification threshold (e.g., warn at 80% of usage).
        /// </summary>
        public double? WarningThresholdPercentage { get; private set; }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
        public User? User { get; private set; }
        public SocialMediaPlatform? Platform { get; private set; }

        protected UserUsageGoal()
        {
        }

        private UserUsageGoal(int id,
            Guid userId,
            int platformId,
            TimeSpan dailyLimit,
            double? warningThresholdPercentage = null) : base(id)
        {
            UserId = userId;
            PlatformId = platformId;
            DailyLimit = dailyLimit;
            WarningThresholdPercentage = warningThresholdPercentage;
        }

        public static UserUsageGoal Create(int id,
            Guid userId,
            int platformId,
            TimeSpan dailyLimit,
            double? warningThresholdPercentage = null)
        {
            return new UserUsageGoal(id,
                userId,
                platformId,
                dailyLimit,
                warningThresholdPercentage);
        }

        public void UpdateLimit(TimeSpan newLimit,
            double? warningThresholdPercentage = null)
        {
            DailyLimit = newLimit;
            WarningThresholdPercentage = warningThresholdPercentage;
            ModifiedOnUtc = DateTime.UtcNow;
            RaiseDomainEvent(new UserUsageGoalUpdatedDomainEvent(Id,
                UserId,
                PlatformId,
                DailyLimit,
                IsActive));
        }

        public void Deactivate()
        {
            IsActive = false;
            ModifiedOnUtc = DateTime.UtcNow;
            RaiseDomainEvent(new UserUsageGoalDeactivatedDomainEvent(Id,
                UserId,
                PlatformId));
        }

        public void Activate()
        {
            IsActive = true;
            ModifiedOnUtc = DateTime.UtcNow;
            RaiseDomainEvent(new UserUsageGoalActivatedDomainEvent(Id,
                UserId,
                PlatformId));
        }
    }
}