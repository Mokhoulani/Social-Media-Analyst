using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public class SocialMediaPlatformConfiguration : IEntityTypeConfiguration<SocialMediaPlatform>
{
    public void Configure(EntityTypeBuilder<SocialMediaPlatform> builder)
    {
        builder.ToTable(TableNames.SocialMediaPlatforms);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IconUrl)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasMany(x => x.UsageRecords)
            .WithOne(x => x.Platform)
            .HasForeignKey(x => x.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UsageSummaries)
            .WithOne(x => x.Platform)
            .HasForeignKey(x => x.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UsageGoals)
            .WithOne(x => x.Platform)
            .HasForeignKey(x => x.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}