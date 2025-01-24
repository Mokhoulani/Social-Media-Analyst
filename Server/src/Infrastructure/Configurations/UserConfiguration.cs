using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, v => Email.Create(v));

        builder
            .Property(x => x.FirstName)
            .HasConversion(x => x.Value, v => FirstName.Create(v))
            .HasMaxLength(FirstName.MaxLength);

        builder
            .Property(x => x.LastName)
            .HasConversion(x => x.Value, v => LastName.Create(v))
            .HasMaxLength(LastName.MaxLength);

        builder.OwnsOne(u => u.Password, passwordBuilder =>
        {
            passwordBuilder.Property(p => p.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });
        
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
