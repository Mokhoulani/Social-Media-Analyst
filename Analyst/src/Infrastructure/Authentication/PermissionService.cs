using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification.Roles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authentication;

public class PermissionService(
    IUnitOfWork unitOfWork) : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        var sep = new UserWithRolesAndPermissionsByIdSpecification(userId);
        var result = await unitOfWork.Repository<User, Guid>()
            .FindOneAsync(sep,
                CancellationToken.None);

        return result.Value.Roles
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public async Task<bool> HasPermissionAsync(Guid userId,
        string permissionName)
    {
        var permissions = await GetPermissionsAsync(userId);
        return permissions.Contains(permissionName,
            StringComparer.OrdinalIgnoreCase);
    }

    public async Task<bool> HasAnyPermissionAsync(Guid userId,
        IEnumerable<string> permissionNames)
    {
        var permissions = await GetPermissionsAsync(userId);
        return permissionNames.Any(permissionName =>
            permissions.Contains(permissionName,
                StringComparer.OrdinalIgnoreCase));
    }

    public async Task<bool> HasAllPermissionsAsync(Guid userId,
        IEnumerable<string> permissionNames)
    {
        var permissions = await GetPermissionsAsync(userId);
        return permissionNames.All(permissionName =>
            permissions.Contains(permissionName,
                StringComparer.OrdinalIgnoreCase));
    }

    public async Task<Result<Role>> GetRoleByNameAsync(string roleName,
        CancellationToken cancellationToken = default)
    {
        var sep = new RoleByNameSpecification(roleName);
        var result = await unitOfWork.Repository<Role, int>()
            .FindOneAsync(sep,
                cancellationToken);
        return result.IsSuccess
            ? result.Value
            : result.Error;
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await unitOfWork.Repository<Role, int>()
            .AsQueryable()
            .FirstOrDefaultAsync(role => role.Id == roleId);
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await unitOfWork.Repository<Role, int>()
            .AsQueryable()
            .AnyAsync(role => role.Name.Equals(roleName,
                StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> RoleExistsAsync(int roleId)
    {
        return await unitOfWork.Repository<Role, int>()
            .AsQueryable()
            .AnyAsync(role => role.Id == roleId);
    }

    public async Task<bool> UserHasRoleAsync(Guid userId,
        string roleName)
    {
        return await unitOfWork.Repository<User, Guid>()
            .AsQueryable()
            .Include(user => user.Roles)
            .AnyAsync(user =>
                user.Id == userId &&
                user.Roles.Any(role => role.Name.Equals(roleName,
                    StringComparison.OrdinalIgnoreCase)));
    }

    public async Task<bool> UserHasRoleAsync(Guid userId,
        int roleId)
    {
        return await unitOfWork.Repository<User, Guid>()
            .AsQueryable()
            .Include(user => user.Roles)
            .AnyAsync(user => user.Id == userId && user.Roles.Any(role => role.Id == roleId));
    }

    public async Task<bool> UserHasAnyRoleAsync(Guid userId,
        IEnumerable<string> roleNames)
    {
        return await unitOfWork.Repository<User, Guid>()
            .AsQueryable()
            .Include(user => user.Roles)
            .AnyAsync(user =>
                user.Id == userId &&
                user.Roles.Any(role => roleNames.Contains(role.Name,
                    StringComparer.OrdinalIgnoreCase)));
    }

    public async Task<bool> UserHasAnyRoleAsync(Guid userId,
        IEnumerable<int> roleIds)
    {
        return await unitOfWork.Repository<User, Guid>()
            .AsQueryable()
            .Include(user => user.Roles)
            .AnyAsync(user => user.Id == userId && user.Roles.Any(role => roleIds.Contains(role.Id)));
    }

    public async Task<bool> UserHasAllRolesAsync(Guid userId,
        IEnumerable<string> roleNames)
    {
        return await unitOfWork.Repository<User, Guid>()
            .AsQueryable()
            .Include(user => user.Roles)
            .AnyAsync(user =>
                user.Id == userId &&
                user.Roles.All(role => roleNames.Contains(role.Name,
                    StringComparer.OrdinalIgnoreCase)));
    }

    public async Task<bool> UserHasAllRolesAsync(Guid userId,
        IEnumerable<int> roleIds)
    {
        return await unitOfWork.Repository<User, Guid>()
            .AsQueryable()
            .Include(user => user.Roles)
            .AnyAsync(user => user.Id == userId && user.Roles.All(role => roleIds.Contains(role.Id)));
    }
}