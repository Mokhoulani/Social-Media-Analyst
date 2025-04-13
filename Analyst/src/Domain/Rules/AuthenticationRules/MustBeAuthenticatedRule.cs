using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;

namespace Domain.Rules.AuthenticationRules
{
    public class MustBeAuthenticatedRule(ICurrentUser currentUser) : Rule
    {
        public override bool IsBroken() => !currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(currentUser.UserId);

        public override Error Error => AuthenticationErrors.User.Unauthenticated;
    }
}