using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable(TableNames.UserDevices);

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DeviceToken)
            .IsRequired()
            .HasMaxLength(512); 

        builder.Property(d => d.CreatedOnUtc)
            .IsRequired();

        builder.Property(d => d.ModifiedOnUtc);

        builder.HasOne(d => d.User)
            .WithMany(u => u.Devices)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasIndex(d => d.DeviceToken)
            .IsUnique(false); 
    }
}
