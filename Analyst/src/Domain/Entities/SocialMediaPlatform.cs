using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class SocialMediaPlatform : Entity<int>,
        IAggregateRoot,
        IAuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string IconUrl { get; private set; } = string.Empty;

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }

        public ICollection<UserSocialMediaUsage> UsageRecords { get; private set; } = [];
        public ICollection<UsageSummary> UsageSummaries { get; private set; } = [];
        public ICollection<UserUsageGoal> UsageGoals { get; private set; } = [];

        protected SocialMediaPlatform()
        {
        }

        private SocialMediaPlatform(string name,
            string iconUrl)
        {
            Name = name;
            IconUrl = iconUrl;
        }


        public static SocialMediaPlatform Create(string name,
            string iconUrl)
        {
            return new SocialMediaPlatform(name,
                iconUrl);
        }
    }
}