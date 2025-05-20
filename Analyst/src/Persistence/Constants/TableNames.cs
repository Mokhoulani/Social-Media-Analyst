using Domain.Entities;
using Persistence.Outbox;

namespace Persistence.Constants;

internal static class TableNames
{
    internal const string Users = nameof(User);
    internal const string OutboxMessages = nameof(OutboxMessage);
    internal const string RefreshTokens = nameof(RefreshToken);
    internal const string PasswordResetTokens = nameof(PasswordResetToken);
    internal const string Roles = nameof(Role);
    internal const string Permissions = nameof(Permission);
    internal const string UserSocialMediaUsages = nameof(UserSocialMediaUsage);
    internal const string SocialMediaPlatforms = nameof(SocialMediaPlatform);
    internal const string Notifications = nameof(Notification);
    internal const string UsageSummaries = nameof(UsageSummary);
    internal const string UserUsageGoals = nameof(UserUsageGoal);
    internal const string UserDevices = nameof(UserDevice);
}
