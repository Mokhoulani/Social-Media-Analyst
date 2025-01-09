namespace Application.Interfaces;

public interface IUserService
{
    Task<int> CreateUserAsync(string email, string name, string timeZone);
    Task UpdateUserProfileAsync(int userId, string name, string timeZone);
    Task DeactivateUserAsync(int userId);
}