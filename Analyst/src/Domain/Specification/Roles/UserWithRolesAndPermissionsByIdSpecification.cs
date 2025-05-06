using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Specification.Roles;

public class UserWithRolesAndPermissionsByIdSpecification : Specification<User, Guid>
{
    public UserWithRolesAndPermissionsByIdSpecification(Guid userId)
    : base(user => user.Id == userId)
    {
        AddInclude(user => user.Roles);

        AddIncludeNavigation(query =>
            query.Include(user => user.Roles)
                 .ThenInclude(role => role.Permissions));
    }
}