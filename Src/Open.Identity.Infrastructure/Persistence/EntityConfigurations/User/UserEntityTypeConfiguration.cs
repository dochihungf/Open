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
        builder
            .HasIndex(u => u.Id);
        
        // Index Clustered 
        builder
            .HasIndex(u => u.Username)
            .IsClustered();

        #endregion
        
        #region Property
        
        builder
            .Property(u => u.Username)
            .HasMaxLength(50);

        builder
            .Property(u => u.PasswordHash)
            .HasMaxLength(255);

        builder
            .Property(u => u.Salt)
            .HasMaxLength(255);

        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        builder
            .Property(u => u.LastName)
            .HasMaxLength(50);
        
        builder.Property(cd => cd.Gender)
            .HasConversion(
                v => v.ToString(), 
                v => (GenderType)Enum.Parse(typeof(GenderType), v) 
            );
        
        #endregion
        
        #region Reference properties

        builder
            .HasOne(u => u.UserConfig)
            .WithOne(uc => uc.User)
            .HasForeignKey<UserConfig>(u => u.OwnerId);
        
        #endregion
    }
}
