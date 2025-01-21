using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;


namespace Application.Services;

public class UserService(IUserRepository userRepository,IUnitOfWork unitOfWork ) : IUserService
{
  
    public async Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken)
    {
       return await userRepository.IsEmailUniqueAsync(email, cancellationToken);
    }
    
    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        userRepository.Insert(user); // Add the user to the repository
        await unitOfWork.SaveChangesAsync(cancellationToken); // Save changes via Unit of Work
        return user; // Return the user with updated properties
    }
 

    public Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
    }
}

