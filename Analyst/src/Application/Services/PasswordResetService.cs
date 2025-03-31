using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification;
using Domain.ValueObjects;

namespace Application.Services;

public class PasswordResetService(
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IUserService userService)
    : IPasswordResetService
{
    public async Task<Result<bool>> RequestPasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(email);

        if (emailResult.IsFailure)
            return DomainErrors.Email.Empty;

        var user = await userService.GetByEmailAsync(emailResult.Value, cancellationToken);

        if (user.IsFailure)
            return DomainErrors.User.NotFound;

        var token = tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        var resetToken = PasswordResetToken.Create(user.Value.Id, token, expiresAt);

        await unitOfWork.Repository<PasswordResetToken, Guid>().AddAsync(resetToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<Result<bool>> ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken)
    {
        var spec = new ValidResetTokenSpecification(token);

        var resetToken = await unitOfWork.Repository<PasswordResetToken, Guid>()
            .FindOneAsync(spec, cancellationToken);

        if (resetToken.IsFailure)
            return DomainErrors.NotFound<PasswordResetToken>();

        var user = await userService.GetUserByIdAsync(resetToken.Value.UserId, cancellationToken);

        if (user.IsFailure)
            return DomainErrors.User.NotFound;

        var passwordResult = Password.Create(newPassword);

        if (passwordResult.IsFailure)
            return DomainErrors.Password.NotValid;

        user.Value.SetPassword(passwordResult.Value);
        await unitOfWork.Repository<User, Guid>().SoftUpdateAsync(user.Value, cancellationToken);

        resetToken.Value.MarkAsUsed();
        await unitOfWork.Repository<PasswordResetToken, Guid>().SoftUpdateAsync(resetToken.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
