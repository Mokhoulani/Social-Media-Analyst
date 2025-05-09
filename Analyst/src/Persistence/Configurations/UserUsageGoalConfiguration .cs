using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations
{
    public class UserUsageGoalConfiguration : IEntityTypeConfiguration<UserUsageGoal>
    {
        public void Configure(EntityTypeBuilder<UserUsageGoal> builder)
        {
            builder.ToTable(TableNames.UserUsageGoals);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PlatformId)
                .IsRequired();

            builder.Property(x => x.DailyLimit)
                .IsRequired();

            builder.Property(x => x.WarningThresholdPercentage)
                .HasPrecision(5,
                    2);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedOnUtc)
                .IsRequired();

            builder.Property(x => x.ModifiedOnUtc)
                .IsRequired(false);

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
                    x.IsActive
                })
                .HasFilter("[IsActive] = 1")
                .IsUnique();

            builder.HasIndex(x => new
            {
                x.UserId,
                x.PlatformId
            });
        }
    }
}