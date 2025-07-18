using Application.Common.Interfaces;
using Application.CQRS.User.Events;
using Application.Services;
using Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authentication.Events;

public class ResetPasswordEventHandler(
    ILogger<UserSignedUpEventHandler> logger,
    IUserService userService,
    EmailService emailService) : INotificationHandler<UserResetPasswordDomainEvent>
{
    public async Task Handle(UserResetPasswordDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(notification.UserId, cancellationToken);

            if (user.IsFailure) throw new Exception("User not found");

            var email = user.Value.Email.Value;

            const string resetLink = $"http://localhost:5014/api/Auth/reset-password";

            await emailService.SendPasswordResetEmailAsync(email, resetLink, cancellationToken);

            logger.LogInformation("Password reset email sent to {Email}", notification);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling UserResetPasswordDomainEvent for UserId: {UserId}",
                notification.UserId);
        }
    }
}