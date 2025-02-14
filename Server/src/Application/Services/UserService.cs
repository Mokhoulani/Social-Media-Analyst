using Application.Common.Interfaces;
using Application.CQRS.User.Commands;
using Domain.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specification;
using Domain.Specification.Users;
using Domain.ValueObjects;


namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<bool> IsEmailExistsAsync(Email email, CancellationToken cancellationToken)
    {
        var spec = new EmailUniqueSpecification(email);
        
        var userExists = await unitOfWork.Repository<User>()
            .ExistsAsync(spec, cancellationToken);
        return !userExists;
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
       var newUser = await unitOfWork.Repository<User>().AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken); 
        return newUser;
    }
    
    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken);
    }
    
    public async Task<User?> LoginAsync(
        LoginCommand command,
         CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.Email);

        var spec = new EmailSpecification(email);
        var user = await unitOfWork.Repository<User>()
            .FindOneAsync(spec, cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");
            
        bool isPasswordValid = user?.VerifyPassword(command.Password) ?? false;

        return isPasswordValid ? user : null;
    }

    public async Task<bool> IsPasswordValidAsync(
       string email,
       string password,
       CancellationToken cancellationToken)
    {
        Email emailResult = Email.Create(email);
        
        var spec = new EmailSpecification(emailResult);
        var user = await unitOfWork.Repository<User>()
            .FindOneAsync(spec, cancellationToken);
        
        return user?.VerifyPassword(password) ?? false;
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        var spec = new EmailSpecification(email);
        var user = await unitOfWork.Repository<User>()
            .FindOneAsync(spec, cancellationToken);
        return user;
    }
}





