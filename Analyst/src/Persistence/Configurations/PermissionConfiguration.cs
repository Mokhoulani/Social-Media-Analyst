﻿using Domain.Entities;
using Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permissions);

        builder.HasKey(p => p.Id);

        IEnumerable<Permission> permissions = Enum
            .GetValues<Domain.Enums.Permission>()
            .Select(p => new Permission((int)p, p.ToString()));  
    }
}
