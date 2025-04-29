namespace Domain.Interfaces;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    System.Security.Claims.ClaimsPrincipal ClaimsPrincipal { get; }
    bool IsInRole(string role);
    string? GetClaim(string claimType);
    ICurrentUser GetCurrentUser();
    IEnumerable<string> GetPermissions();
}