namespace Domain.Enums;

public enum NotificationType
{
    UsageLimitWarning = 1,
    UsageLimitReached = 2,
    DailySummary = 3,
    WeeklySummary = 4,
    MonthlySummary = 5,
    GoalAchievement = 6,
    GoalMissed = 7,
    SystemAlert = 8
}