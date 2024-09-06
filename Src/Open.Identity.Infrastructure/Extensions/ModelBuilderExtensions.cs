using Open.Core.SeedWork.Interfaces;
using Open.Identity.Domain.Entities;
using Open.Identity.Domain.Enums;
using Open.Identity.Infrastructure.Options;

namespace Open.Identity.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    private static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, TableConfiguration configuration) where TEntity : class
    {
        return string.IsNullOrWhiteSpace(configuration.Schema) ? entityTypeBuilder.ToTable(configuration.Name) : entityTypeBuilder.ToTable(configuration.Name, configuration.Schema);
    }
    
    private static EntityTypeBuilder<TEntity> ApplyEntityAuditConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : EntityAuditBase
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(e => e.LastModifiedBy).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired(false);
        builder.Property(e => e.LastModifiedDate).IsRequired(false);
        builder.Property(e => e.DeletedBy).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired(false);
        builder.Property(e => e.DeletedDate).IsRequired(false);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        
        return builder;
    }
    
    private static EntityTypeBuilder<TEntity> ApplyEntityConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : EntityAuditBase
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(DataSchemaLength.ExtraLarge);
        builder.Property(e => e.LastModifiedBy).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired(false);
        builder.Property(e => e.LastModifiedDate).IsRequired(false);
        builder.Property(e => e.DeletedBy).HasMaxLength(DataSchemaLength.ExtraLarge).IsRequired(false);
        builder.Property(e => e.DeletedDate).IsRequired(false);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        
        return builder;
    }

    public static void ConfigurePersistedGrantContext(this ModelBuilder builder, IdentityStoreOptions storeOptions)
    {
        if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema))
        {
            builder.HasDefaultSchema(storeOptions.DefaultSchema);
        }

        builder.Entity<User>(user =>
        {
            // Non - Index Clustered   
            user.HasIndex(u => u.Id);

            // Index Clustered 
            user.HasIndex(u => u.Username).IsClustered();
            
            user.Property(u => u.Username).HasMaxLength(DataSchemaLength.Medium);
            user.Property(u => u.PasswordHash).HasMaxLength(DataSchemaLength.ExtraLarge);
            user.Property(u => u.Salt).HasMaxLength(DataSchemaLength.ExtraLarge);
            user.Property(u => u.PhoneNumber).HasMaxLength(DataSchemaLength.Small).IsUnicode(false);
            user.Property(u => u.Email).HasMaxLength(DataSchemaLength.ExtraLarge).IsUnicode(false);
            user.Property(u => u.FirstName).HasMaxLength(DataSchemaLength.Medium);
            user.Property(u => u.LastName).HasMaxLength(DataSchemaLength.Medium);
            user.Property(cd => cd.Gender).HasConversion(v => v.ToString(), v => (GenderType)Enum.Parse(typeof(GenderType), v));
            
            user.HasOne(u => u.UserConfig).WithOne(uc => uc.User).HasForeignKey<UserConfig>(u => u.OwnerId);
            user.HasOne(u => u.SecretKey).WithOne(uc => uc.User).HasForeignKey<SecretKey>(u => u.OwnerId);
            user.HasOne(u => u.OTP).WithOne(uc => uc.User).HasForeignKey<OTP>(u => u.OwnerId);
            user.HasOne(u => u.MFA).WithOne(uc => uc.User).HasForeignKey<MFA>(u => u.OwnerId);
            user.HasMany(u => u.SignInHistories).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);

            user.ApplyEntityAuditConfiguration();

        });
    }
}
