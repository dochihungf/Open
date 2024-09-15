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
    
    private static EntityTypeBuilder<TEntity> ApplyEntityConfiguration<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : EntityBase
    {
        return builder;
    }
    
    public static void ModelBuilderIdentityContext(this ModelBuilder builder, IdentityStoreOptions storeOptions)
    {
        if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema))
        {
            builder.HasDefaultSchema(storeOptions.DefaultSchema);
        }

        // User builder
        builder.Entity<User>(user =>
        {
            user.ToTable(storeOptions.User);
            
            user.HasKey(e => e.Id);
            user.Property(u => u.Username).HasMaxLength(DataSchemaLength.Medium);
            user.Property(u => u.PasswordHash).HasMaxLength(DataSchemaLength.ExtraLarge);
            user.Property(u => u.Salt).HasMaxLength(DataSchemaLength.ExtraLarge);
            user.Property(u => u.PhoneNumber).HasMaxLength(DataSchemaLength.Small).IsUnicode(false);
            user.Property(u => u.Email).HasMaxLength(DataSchemaLength.ExtraLarge).IsUnicode(false);
            user.Property(u => u.FirstName).HasMaxLength(DataSchemaLength.Medium);
            user.Property(u => u.LastName).HasMaxLength(DataSchemaLength.Medium);
            user.Property(cd => cd.Gender).HasConversion(v => v.ToString(), v => (GenderType)Enum.Parse(typeof(GenderType), v));
            user.ApplyEntityAuditConfiguration();
            
            // Non - Index Clustered   
            user.HasIndex(u => u.Id);

            // Index Clustered 
            user.HasIndex(u => u.Username).IsClustered();
            
            user.HasOne(u => u.UserConfig).WithOne(uc => uc.User).HasForeignKey<UserConfig>(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            user.HasOne(u => u.SecretKey).WithOne(uc => uc.User).HasForeignKey<SecretKey>(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            user.HasOne(u => u.MFA).WithOne(uc => uc.User).HasForeignKey<MFA>(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            user.HasMany(u => u.OTPs).WithOne(uc => uc.User).HasForeignKey(u => u.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            user.HasMany(u => u.SignInHistories).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            user.HasMany(u => u.RefreshTokens).WithOne(ur => ur.User).HasForeignKey(ur => ur.OwnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        });
        
        // UserConfig builder
        builder.Entity<UserConfig>(userConfig =>
        {
            userConfig.ToTable(storeOptions.UserConfig);
            
            userConfig.ApplyEntityConfiguration();
            
            userConfig.HasIndex(r => r.OwnerId);
        });
        
        // Role builder
        builder.Entity<Role>(role =>
        {
            role.ToTable(storeOptions.Role);
            
            role.HasKey(e => e.Id);
            role.Property(r => r.Code).HasMaxLength(DataSchemaLength.Medium);
            role.Property(r => r.Name).HasMaxLength(DataSchemaLength.Large);
            role.ApplyEntityAuditConfiguration();
            
            role.HasIndex(r => r.Code);
        });
        
        // UserRole builder
        builder.Entity<UserRole>(userRole =>
        {
            userRole.ToTable(storeOptions.UserRole);
            
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
            userRole.ApplyEntityAuditConfiguration();
            
            userRole.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        });
        
        // Permission builder
        builder.Entity<Permission>(permission =>
        {
            permission.ToTable(storeOptions.Permission);
            
            permission.HasKey(a => a.Id);
            permission.Property(a => a.Code).HasMaxLength(DataSchemaLength.Medium).IsUnicode(false);
            permission.Property(a => a.Name).HasMaxLength(DataSchemaLength.Large);
            permission.Property(a => a.Description).HasMaxLength(DataSchemaLength.ExtraLarge);
            permission.ApplyEntityAuditConfiguration();
            
            permission.HasIndex(r => r.Code);
            
        });
        
        // RolePermission builder
        builder.Entity<RolePermission>(rolePermission =>
        {
            rolePermission.ToTable(storeOptions.RolePermission);
            
            rolePermission.HasKey(ur => new { ur.PermissionId, ur.RoleId });
            rolePermission.ApplyEntityAuditConfiguration();
            
            rolePermission.HasOne(ur => ur.Role).WithMany(u => u.RolePermissions).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            rolePermission.HasOne(ur => ur.Permission).WithMany(r => r.RolePermissions).HasForeignKey(ur => ur.PermissionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            
            rolePermission.HasIndex(ur => new { ur.PermissionId, ur.RoleId });
        });
        
        // UserPermission builder
        builder.Entity<UserPermission>(userPermission =>
        {
            userPermission.ToTable(storeOptions.UserPermission);
            
            userPermission.HasKey(ur => new { ur.UserId, ur.PermissionId });
            userPermission.ApplyEntityAuditConfiguration();
            
            userPermission.HasOne(up => up.User).WithMany(u => u.UserPermissions).HasForeignKey(up => up.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            userPermission.HasOne(up => up.Permission).WithMany(u => u.UserPermissions).HasForeignKey(up => up.PermissionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            
            userPermission.HasIndex(ur => new { ur.PermissionId, ur.UserId });
        });

        // RefreshToken builder
        builder.Entity<RefreshToken>(refreshToken =>
        {
            refreshToken.ToTable(storeOptions.RefreshToken);
            
            refreshToken.HasKey(e => e.Id);
            refreshToken.Property(e => e.RefreshTokenValue).HasMaxLength(DataSchemaLength.SuperLarge);
            refreshToken.Property(e => e.CurrentAccessToken).HasMaxLength(DataSchemaLength.SuperLarge);
            
            refreshToken.HasIndex(e => e.RefreshTokenValue);
            refreshToken.HasIndex(e => new { e.OwnerId, e.RefreshTokenValue });
            refreshToken.HasIndex(e => e.CurrentAccessToken);
            refreshToken.HasIndex(e => new { e.OwnerId, e.CurrentAccessToken });
        });
        
        // SecretKey builder
        builder.Entity<SecretKey>(secretKey =>
        {
            secretKey.ToTable(storeOptions.SecretKey);
            
            secretKey.HasKey(e => e.Id);
            secretKey.Property(e => e.Key).HasMaxLength(DataSchemaLength.Medium).IsUnicode();
            
            secretKey.HasIndex(e => e.OwnerId);
            secretKey.HasIndex(e => e.Key);
            secretKey.HasIndex(e => new { e.OwnerId, e.Key });
        });
        
        // OTP builder
        builder.Entity<OTP>(OTP =>
        {
            OTP.ToTable(storeOptions.OTP);
            
            OTP.HasKey(e => e.Id);
            OTP.Property(e => e.Code).HasMaxLength(DataSchemaLength.Medium).IsUnicode(false);
            OTP.Property(e => e.Type).HasConversion(v => v.ToString(), v => (OTPType)Enum.Parse(typeof(OTPType), v));
            OTP.ApplyEntityAuditConfiguration();
            
            OTP.HasIndex(e => e.OwnerId);
            OTP.HasIndex(e => e.Code);
            OTP.HasIndex(e => new { e.OwnerId, e.Code, e.Type });
        });
        
        
        // MFA builder
        builder.Entity<MFA>(MFA =>
        {
            MFA.ToTable(storeOptions.MFA);
            
            MFA.HasKey(e => e.Id);
            MFA.Property(e => e.Type).HasConversion(v => v.ToString(), v => (MFAType)Enum.Parse(typeof(MFAType), v));
            MFA.ApplyEntityAuditConfiguration();

            MFA.HasIndex(e => e.OwnerId);
        });
        
        // Log builder
        builder.Entity<SignInHistory>(history =>
        {
            history.ToTable(storeOptions.SignInHistory);
            
            history.HasKey(e => e.Id);
            history.ApplyEntityConfiguration();
            
            // history.HasIndex(e => new {e.UserId, e.Ip, e.Browser, e.Device});
        });

    }
}
