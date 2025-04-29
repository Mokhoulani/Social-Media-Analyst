using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Rules.AuthorizationRules;

public class MustBeAuthorizationRule(ICurrentUser currentUser, string requiredPermission) : Rule
{
    public override bool IsBroken() =>
        !currentUser.IsAuthenticated ||
        string.IsNullOrWhiteSpace(currentUser.UserId) ||
        !currentUser.GetPermissions().Contains(requiredPermission);

    public override Error Error => AuthorizationErrors.Forbidden(requiredPermission);
}
