namespace Application.Common.Interfaces
{
    public interface IFirebaseNotificationService
    {
        Task<string> SendAsync(string title, string body, string deviceToken);
    }
}