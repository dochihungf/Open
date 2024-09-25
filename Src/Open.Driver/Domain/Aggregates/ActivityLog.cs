using Open.Core.SeedWork.Interfaces;
using Open.Driver.Domain.Enums;

namespace Open.Driver.Domain.Aggregates;

public class ActivityLog : IAggregateRoot
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ActionType ActionType { get; set; }
    public Guid ResourceId { get; set; }
    public ResourceType ResourceType { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public Guid? SourceId { get; set; } 
    public Guid? DestinationId { get; set; } 
    public DateTime Timestamp { get; set; }
}
