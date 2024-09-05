using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class OTPEntityTypeConfiguration : PersonalizedEntityAuditBaseEntityTypeConfiguration<OTP>
{
    public override void Configure(EntityTypeBuilder<OTP> builder)
    {
        base.Configure(builder);

        builder
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Code)
            .HasMaxLength(DataSchemaLength.Medium)
            .IsUnicode(false);
    }
}
