using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IRepository<Domain.Entities.User>
{
    public async Task<User?> AddAsync(User user, CancellationToken cancellationToken)
    {
        var userCreated = await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return userCreated?.Entity;
    }
    
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Users.FindAsync(id, cancellationToken);
    }

    public async Task<IList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Users.ToListAsync(cancellationToken);
    }
    
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users
            .AnyAsync(u => u.Email == email, cancellationToken);
    }
}