using System.Security.Claims;
using Domain.Interfaces;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure.Service;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
    
    public string? UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email) ?? User?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public string? Role => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

    public ClaimsPrincipal ClaimsPrincipal => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    public bool IsInRole(string role) => httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;

    public string? GetClaim(string claimType) => httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);

    public ICurrentUser GetCurrentUser() => this;
    
    public IEnumerable<string> GetPermissions()
    {
        return ClaimsPrincipal?.FindAll(CustomClaims.Permissions).Select(c => c.Value) ?? Enumerable.Empty<string>();
    }
}