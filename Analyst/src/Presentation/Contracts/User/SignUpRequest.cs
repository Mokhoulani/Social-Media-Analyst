namespace presentation.Contracts.User;

public record SignUpRequest(
    string FirstName,
    string Email,
    string LastName,
    string Password);