using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(User user, CancellationToken cancellationToken = default);
}
