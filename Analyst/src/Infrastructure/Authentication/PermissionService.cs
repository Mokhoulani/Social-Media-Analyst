using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Authentication;

public class PermissionService(IUnitOfWork unitOfWork) : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        var roles = await unitOfWork.Repository<User,Guid>().AsQueryable()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Roles) 
            .ToListAsync();
        
        return roles
            .SelectMany(role => role.Permissions) 
            .Select(permission => permission.Name) 
            .ToHashSet(StringComparer.OrdinalIgnoreCase); 
    }
}
