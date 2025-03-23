using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public class PasswordResetTokenConfiguration: IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable(TableNames.PasswordResetTokens);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId);
        
        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasIndex(x => x.Token)
            .IsUnique();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.PasswordResetTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}