using Domain.Entities;
using Domain.Shared;

namespace Domain.Interfaces;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(Guid userId);
    Task<bool> HasPermissionAsync(Guid userId, string permissionName);
    Task<Result<Role>> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default);
}
