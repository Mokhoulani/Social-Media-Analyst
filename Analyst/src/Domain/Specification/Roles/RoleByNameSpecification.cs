using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Specification.Roles;

public sealed class RoleByNameSpecification(string roleName)
    : Specification<Role, int>(role => EF.Functions.Like(role.Name, roleName))
{
}