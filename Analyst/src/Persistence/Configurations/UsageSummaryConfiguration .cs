using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public class UsageSummaryConfiguration : IEntityTypeConfiguration<UsageSummary>
{
    public void Configure(EntityTypeBuilder<UsageSummary> builder)
    {
        builder.ToTable(TableNames.UsageSummaries);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.PlatformId)
            .IsRequired();

        builder.Property(x => x.TotalDuration)
            .IsRequired();

        builder.Property(x => x.SummaryDate)
            .HasConversion(
                v => v.ToDateTime(TimeOnly.MinValue),
                v => DateOnly.FromDateTime(v))
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Platform)
            .WithMany()
            .HasForeignKey(x => x.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
            {
                x.UserId,
                x.PlatformId,
                x.SummaryDate
            })
            .IsUnique();
    }
}