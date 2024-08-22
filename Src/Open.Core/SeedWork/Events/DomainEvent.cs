using MediatR;

namespace Open.Core.SeedWork.Events;

public abstract class DomainEvent : INotification
{
    public DateTime DateOccurred { get; protected set; }
}
