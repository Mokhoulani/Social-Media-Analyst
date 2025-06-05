using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Common.Interfaces;

public interface IUserService
{
    Task<Result<bool>> IsEmailExistsAsync(Email email, CancellationToken cancellationToken);
    Task<Result<User>> AddUserAsync(User user, CancellationToken cancellationToken);
    Task<Result<User>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Result<User>> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsPasswordValidAsync(string email, string password, CancellationToken cancellationToken= default);
    Task<Result<User>> GetByEmailAsync(Email user, CancellationToken cancellationToken = default);
}