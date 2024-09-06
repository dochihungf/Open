using Open.Identity.Domain.Entities;
using Open.Identity.Domain.Enums;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class UserEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        // EntityAuditBase
        base.Configure(builder);
        
        #region Indexes

        // Non - Index Clustered   
        builder.HasIndex(u => u.Id);
        
        // Index Clustered 
        builder.HasIndex(u => u.Username).IsClustered();

        #endregion
        
        #region Property
        
        builder.Property(u => u.Username).HasMaxLength(DataSchemaLength.Medium);
        builder.Property(u => u.PasswordHash).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(u => u.Salt).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(u => u.PhoneNumber).HasMaxLength(DataSchemaLength.Small).IsUnicode(false);
        builder.Property(u => u.Email).HasMaxLength(DataSchemaLength.ExtraLarge).IsUnicode(false);
        builder.Property(u => u.FirstName).HasMaxLength(DataSchemaLength.Medium);
        builder.Property(u => u.LastName).HasMaxLength(DataSchemaLength.Medium);
        builder.Property(cd => cd.Gender).HasConversion(v => v.ToString(), v => (GenderType)Enum.Parse(typeof(GenderType), v));
        
        #endregion
        
        #region Reference properties

        builder.HasOne(u => u.UserConfig).WithOne(uc => uc.User).HasForeignKey<UserConfig>(u => u.OwnerId);
        builder.HasOne(u => u.SecretKey).WithOne(uc => uc.User).HasForeignKey<SecretKey>(u => u.OwnerId);
        builder.HasOne(u => u.OTP).WithOne(uc => uc.User).HasForeignKey<OTP>(u => u.OwnerId);
        builder.HasOne(u => u.MFA).WithOne(uc => uc.User).HasForeignKey<MFA>(u => u.OwnerId);
        builder.HasMany(u => u.SignInHistories).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);
        
        #endregion
    }
}
