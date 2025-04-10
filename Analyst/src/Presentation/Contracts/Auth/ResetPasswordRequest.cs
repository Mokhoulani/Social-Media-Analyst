namespace presentation.Contracts.Auth;

public record ResetPasswordRequest(string Token, string Password);