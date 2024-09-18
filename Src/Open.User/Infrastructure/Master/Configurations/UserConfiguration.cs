using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Open.Constants;
using Open.User.Domain.Enums;
using Open.User.Domain.UserAggregate;

namespace Open.User.Infrastructure.Master.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<Domain.UserAggregate.User>
{
    public void Configure(EntityTypeBuilder<Domain.UserAggregate.User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(u => u.Username).HasMaxLength(DataSchemaLength.Medium);
        builder.Property(u => u.PasswordHash).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(u => u.Salt).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(u => u.PhoneNumber).HasMaxLength(DataSchemaLength.Small).IsUnicode(false);
        builder.Property(u => u.Email).HasMaxLength(DataSchemaLength.ExtraLarge).IsUnicode(false);
        builder.Property(u => u.FirstName).HasMaxLength(DataSchemaLength.Medium);
        builder.Property(u => u.LastName).HasMaxLength(DataSchemaLength.Medium);
        builder.Property(cd => cd.Gender).HasConversion(v => v.ToString(), v => (GenderType)Enum.Parse(typeof(GenderType), v));
            
        // Non - Index Clustered   
        builder.HasIndex(u => u.Id);
        
        builder.Property(e => e.CreatedBy).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(e => e.LastModifiedBy).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired(false);
        builder.Property(e => e.LastModifiedDate).IsRequired(false);
        builder.Property(e => e.DeletedBy).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired(false);
        builder.Property(e => e.DeletedDate).IsRequired(false);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        
        builder.HasMany(u => u.Addresses).WithOne(e => e.User).HasForeignKey(ur => ur.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(u => u.Profile).WithOne(e => e.User).HasForeignKey<UserProfile>(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(u => u.Config).WithOne(e => e.User).HasForeignKey<UserConfig>(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(u => u.Avatar).WithOne(e => e.User).HasForeignKey<UserAvatar>(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}