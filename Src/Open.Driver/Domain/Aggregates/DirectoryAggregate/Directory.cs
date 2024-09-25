using Open.Driver.Domain.SeedWork;

namespace Open.Driver.Domain.Aggregates;

public class Directory : EntityBase
{
    public string Name { get; set; }

    public long ParentId { get; set; }

    public string Path { get; set; }

    public int DuplicateNo { get; set; }
    
    public virtual Directory Parent { get; set; }
    
    public ICollection<Directory> SubDirectories { get; set; }

    public ICollection<File> Files { get; set; } 
}
