using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public void Insert(User user) => context.Set<User>().Add(user);
    public Task<User?> GetByIdAsync(Guid userId)
    {
        return context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)

    {
        return await context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return !await context.Set<User>().AnyAsync(u => u.Email == email, cancellationToken);
    }
}