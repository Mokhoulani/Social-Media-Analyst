using Application.CQRS.User.Events;
using Application.Services;
using Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Authentication.Events;

public class ResetPasswordEventHandler(
    ILogger<UserSignedUpEventHandler> logger,
    EmailService emailService)
    : INotificationHandler<UserResetPasswordDomainEvent>
{
    public async Task Handle(
        UserResetPasswordDomainEvent notification,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Handling UserResetPasswordDomainEvent for UserId: {UserId}", notification.UserId);

            // Generate a reset password link (Assuming there's a method for this)
            string resetLink = $"https://yourapp.com/reset-password?token={notification}";

            // Send the password reset email
            await emailService.SendPasswordResetEmailAsync(
                "mo@gmail.com",
                resetLink,
                cancellationToken);

            logger.LogInformation("Password reset email sent to {Email}", notification);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling UserResetPasswordDomainEvent for UserId: {UserId}", notification.UserId);
            throw;
        }
    }
}