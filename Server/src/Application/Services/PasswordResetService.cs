using Application.Common.Interfaces;
using Domain.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specification;
using Domain.ValueObjects;

namespace Application.Services;

public class PasswordResetService(
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IUserService userService)
    : IPasswordResetService
{
    public async Task<bool> RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(email);
        var user = await userService.GetByEmailAsync(emailResult, cancellationToken);
        
        if (user == null)
            throw new NotFoundException("User not found");

        var token =  tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        var resetToken = PasswordResetToken.Create(user.Id, token, expiresAt);
        
        await unitOfWork.Repository<PasswordResetToken>().AddAsync(resetToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken)
    {
        var spec = new ValidResetTokenSpecification(token);
        
        var resetToken = await unitOfWork.Repository<PasswordResetToken>()
            .FindOneAsync(spec, cancellationToken);
    
        if (resetToken == null)
           return false;
        
        var user = await userService.GetUserByIdAsync(resetToken.UserId, cancellationToken);
        
        if (user == null) 
           return false;
        
        var passwordResult = Password.Create(newPassword);
        user.SetPassword(passwordResult);
        
        await unitOfWork.Repository<User>().SoftUpdateAsync(user, cancellationToken);
        resetToken.MarkAsUsed();
        await unitOfWork.Repository<PasswordResetToken>().SoftUpdateAsync(resetToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        resetToken.MarkAsUsed();
        await unitOfWork.Repository<PasswordResetToken>().AddAsync(resetToken, cancellationToken);
        return true;
    }
}
