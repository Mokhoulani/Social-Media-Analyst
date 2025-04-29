using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Common.CustomAttributes;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString())
    {
    }
}
