using Open.Driver.Domain.SeedWork;

namespace Open.Driver.Domain.Aggregates;

public class FileMetadata : EntityBase
{
    public Guid FileId { get; set; } 
    
    public virtual File File { get; set; } = null!;
}
