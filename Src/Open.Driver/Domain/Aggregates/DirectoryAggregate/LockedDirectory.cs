using Open.Driver.Domain.SeedWork;

namespace Open.Driver.Domain.Aggregates;

public class LockedDirectory : EntityBase
{
    public Guid DirectoryId { get; set; }

    public bool EnabledLock { get; set; }

    public string Password { get; set; }
    
    public virtual Directory Directory { get; set; }
}
