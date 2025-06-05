namespace Presentation.Contracts.Goal;

public record CreateOrUpdateUserUsageGoalRequest(
    int Id,
    string UserId,
    int PlatformId,
    string DailyLimit);
    