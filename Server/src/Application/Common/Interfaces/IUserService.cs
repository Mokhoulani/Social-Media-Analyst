using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Interfaces;

public interface IUserService
{
    Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
}