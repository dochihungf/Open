using Open.Identity.Domain.Entities;

namespace Open.Identity.Infrastructure.Persistence.EntityConfigurations.History;

public class SignInHistoryEntityTypeConfiguration: EntityBaseEntityTypeConfiguration<SignInHistory>
{
    public override void Configure(EntityTypeBuilder<SignInHistory> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(s => s.Id);
    }
}
