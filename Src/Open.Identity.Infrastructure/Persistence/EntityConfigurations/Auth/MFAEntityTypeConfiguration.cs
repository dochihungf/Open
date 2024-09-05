using Open.Identity.Domain.Entities;
using Open.Identity.Domain.Enums;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations;

public class MFAEntityTypeConfiguration : PersonalizedEntityAuditBaseEntityTypeConfiguration<MFA>
{
    public override void Configure(EntityTypeBuilder<MFA> builder)
    {
        base.Configure(builder);
        
        builder.Property(cd => cd.Type)
            .HasConversion(
                v => v.ToString(), 
                v => (MFAType)Enum.Parse(typeof(MFAType), v) 
            );
    }
}
