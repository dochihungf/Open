using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class RefreshTokenEntityTypeConfiguration : EntityAuditBaseEntityTypeConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(a => a.Id);
        
        builder
            .Property(a => a.RefreshTokenValue)
            .HasMaxLength(DataSchemaLength.SuperLarge);
       
        builder
            .Property(a => a.CurrentAccessToken)
            .HasMaxLength(DataSchemaLength.SuperLarge);
        
        builder
            .HasOne(u => u.User)
            .WithMany(uc => uc.RefreshTokens)
            .HasForeignKey(u => u.OwnerId);
    }
}
