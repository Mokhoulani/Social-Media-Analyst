using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface IUserRepository
{
    void Insert(User user);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
}