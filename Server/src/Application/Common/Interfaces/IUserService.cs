using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces;

public interface IUserService
{
    Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
    Task<bool> IsPasswordValidAsync(string email, string password, CancellationToken cancellationToken= default);
    Task<User?> GetByEmailAsync(Email user, CancellationToken cancellationToken = default);
}