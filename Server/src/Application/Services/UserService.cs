using Application.Common.Interfaces;
using Application.CQRS.User.Commands;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification.Users;
using Domain.ValueObjects;


namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Result<bool>> IsEmailExistsAsync(Email email, CancellationToken cancellationToken)
    {
        var spec = new EmailUniqueSpecification(email);
        
        var userExists = await unitOfWork.Repository<User>()
            .ExistsAsync(spec, cancellationToken);
        
        if(userExists.IsFailure)
            return (Result<bool>)Result.Failure(userExists.Error);
        
        return Result.Success(userExists.Value);
    }

    public async Task<Result<User>> AddUserAsync(User user, CancellationToken cancellationToken)
    {
       var newUser = await unitOfWork.Repository<User>().AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken); 
        return newUser;
    }
    
    public async Task<Result<User>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userResult = await unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken);
        
        if (userResult.IsFailure)
         return (Result<User>)Result.Failure(DomainErrors.User.NotFound);
        
        return Result.Success(userResult.Value);
    }
    
    public async Task<Result<User>> LoginAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        Result<Email> emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
            return Result.Failure<User>(emailResult.Error);
        

        var spec = new EmailSpecification(emailResult.Value);
        var user = await unitOfWork.Repository<User>()
            .FindOneAsync(spec, cancellationToken);

        if (user.IsFailure)
            return Result.Failure<User>(DomainErrors.User.NotFound);


        bool isPasswordValid = user.Value.VerifyPassword(command.Password);

        if (!isPasswordValid)
            return Result.Failure<User>(DomainErrors.User.InvalidCredentials);

        return Result.Success(user.Value);
    }
    
    public async Task<Result<bool>> IsPasswordValidAsync(
       string email,
       string password,
       CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(email);

        if (emailResult.IsFailure)
            return Result.Failure<bool>(emailResult.Error);
        
        var spec = new EmailSpecification(emailResult.Value);
        var user = await unitOfWork.Repository<User>()
            .FindOneAsync(spec, cancellationToken);
        
        return Result.Success(user.IsFailure && user.Value.VerifyPassword(password));
    }

    public async Task<Result<User>> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        var spec = new EmailSpecification(email);
        var user = await unitOfWork.Repository<User>()
            .FindOneAsync(spec, cancellationToken);
        
        return user.IsFailure ? Result.Failure<User>(DomainErrors.User.NotFound) 
            : Result.Success(user.Value);
    }
}





