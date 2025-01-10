using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;


namespace Application.Services;

public class UserService(IRepository<User> userRepository) : IUserService
{
    public async Task<User?> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        return await userRepository.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
         return await userRepository.GetByIdAsync(id, cancellationToken);
    }
    
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await userRepository.EmailExistsAsync(email, cancellationToken);
    }
}

