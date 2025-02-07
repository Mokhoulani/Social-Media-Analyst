using Application.Services;
using Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.User.Handlers;

public class UserSignedUpEventHandler(ILogger<UserSignedUpEventHandler> logger, EmailService emailService)
    : INotificationHandler<UserSignedUpDomainEvent>
{
    public async Task Handle(
        UserSignedUpDomainEvent notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("User signed up with ID and Email ");
            // Sending the welcome email
        var subject = "Welcome to Our Platform!";
        var body = $"Hello {notification},\n\nThank you for signing up! We're excited to have you on board.";
    
        // Await the email sending process to ensure it completes before moving on
        await emailService.SendEmailAsync("mo@gmail.com", subject, body);

        await Task.CompletedTask;
    }
}



