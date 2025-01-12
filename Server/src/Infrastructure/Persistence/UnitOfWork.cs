﻿using Domain.Interfaces;

namespace Infrastructure.Persistence;
internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}