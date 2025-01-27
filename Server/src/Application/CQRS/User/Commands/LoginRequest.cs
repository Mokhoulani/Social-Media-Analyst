namespace Application.CQRS.User.Commands;

public record LoginRequest(string Email,string Password);
