using Application.Common.Interfaces;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;


namespace Application.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
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


    public async Task<User?> LoginAsync(
        LoginCommand command,
         CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.Email);

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        bool isPasswordValid = user?.VerifyPassword(command.Password) ?? false;

        return isPasswordValid ? user : null;
    }

    public async Task<bool> IsPasswordValidAsync(
       string email,
       string password,
       CancellationToken cancellationToken)
    {
        Email emailResult = Email.Create(email);

        var user = await userRepository.GetByEmailAsync(emailResult, cancellationToken);

        return user?.VerifyPassword(password) ?? false;
    }

}





