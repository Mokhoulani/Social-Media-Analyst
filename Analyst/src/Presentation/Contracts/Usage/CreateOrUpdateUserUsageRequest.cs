namespace Presentation.Contracts.Usage
{
    public record CreateOrUpdateUserUsageRequest(
        int Id,
        string UserId,
        int PlatformId,
        string StartTime);
}