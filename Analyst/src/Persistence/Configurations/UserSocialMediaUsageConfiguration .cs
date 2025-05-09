using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations
{
    public class UserSocialMediaUsageConfiguration : IEntityTypeConfiguration<UserSocialMediaUsage>
    {
        public void Configure(EntityTypeBuilder<UserSocialMediaUsage> builder)
        {
            builder.ToTable(TableNames.UserSocialMediaUsages);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PlatformId)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.EndTime)
                .HasColumnType("datetime2");

            builder.HasOne(x => x.User)
                .WithMany(u => u.SocialMediaUsages)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Platform)
                .WithMany(p => p.UsageRecords)
                .HasForeignKey(x => x.PlatformId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.PlatformId,
                x.StartTime
            });
        }
    }
}