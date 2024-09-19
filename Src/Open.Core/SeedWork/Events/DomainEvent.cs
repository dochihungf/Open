using MediatR;

namespace Open.Core.SeedWork.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime DateOccurred { get; protected set; }
}
