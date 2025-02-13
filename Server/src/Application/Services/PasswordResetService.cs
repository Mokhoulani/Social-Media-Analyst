using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specification;
using Domain.ValueObjects;

namespace Application.Services;

public class PasswordResetService(
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IUserRepository userRepository)
    : IPasswordResetService
{
    public async Task RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(email);
        var user = await userRepository.GetByEmailAsync(emailResult, cancellationToken);
        if (user == null) return;

        var token =  tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        var resetToken = PasswordResetToken.Create(user.Id, token, expiresAt);
        
        await unitOfWork.Repository<PasswordResetToken>().AddAsync(resetToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken)
    {
        var spec = new ValidResetTokenSpecification(token);
        
        var resetToken = await unitOfWork.Repository<PasswordResetToken>()
            .FindOneAsync(spec, cancellationToken);
    
        if (resetToken == null)
            throw new InvalidOperationException("Invalid or expired token.");

        var user = await userRepository.GetByIdAsync(resetToken.UserId, cancellationToken);
        if (user == null) throw new InvalidOperationException("User not found.");

       var passwordResult = Password.Create(newPassword);
        user.SetPassword(passwordResult);
        
        await unitOfWork.Repository<User>().SoftUpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        resetToken.MarkAsUsed();
        await unitOfWork.Repository<PasswordResetToken>().AddAsync(resetToken, cancellationToken);
    }
    
}
