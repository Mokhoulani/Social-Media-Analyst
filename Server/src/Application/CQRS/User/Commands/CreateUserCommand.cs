using MediatR;

namespace Application.CQRS.User.Commands;
public record CreateUserCommand(string Email, string Name, string TimeZone) : IRequest<int>;