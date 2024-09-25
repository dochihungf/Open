using Open.Core.SeedWork.Interfaces;
using Open.Driver.Domain.SeedWork;

namespace Open.Driver.Domain.Aggregates;

public class Config : EntityBase, IAggregateRoot
{
    public long MaxCapacity { get; set; }
    public long MaxFileSize { get; set; }
}
