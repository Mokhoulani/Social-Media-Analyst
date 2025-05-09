using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class UsageSummary : Entity<int>,
        IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public int PlatformId { get; private set; }

        /// <summary>
        /// The total duration spent on the platform for this date.
        /// </summary>
        public TimeSpan TotalDuration { get; private set; }

        /// <summary>
        /// The date this summary refers to.
        /// Typically represents a single day (or could be adapted for weekly/monthly).
        /// </summary>
        public DateOnly SummaryDate { get; private set; }
        
        public User? User { get; private set; }
        public SocialMediaPlatform? Platform { get; private set; }
        
        protected UsageSummary()
        {
        }

        public UsageSummary(Guid userId,
            int platformId,
            DateOnly summaryDate,
            TimeSpan initialDuration)
        {
            UserId = userId;
            PlatformId = platformId;
            SummaryDate = summaryDate;
            TotalDuration = initialDuration;
        }

        public static UsageSummary Create(Guid userId,
            int platformId,
            DateOnly summaryDate,
            TimeSpan initialDuration)
        {
            return new UsageSummary(userId,
                platformId,
                summaryDate,
                initialDuration);
        }

        public void AddDuration(TimeSpan additionalDuration)
        {
            TotalDuration += additionalDuration;
        }

        public void ResetDuration()
        {
            TotalDuration = TimeSpan.Zero;
        }
    }
}