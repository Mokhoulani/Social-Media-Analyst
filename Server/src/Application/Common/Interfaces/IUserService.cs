using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
}