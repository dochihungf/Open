using Open.MessageBroker.Abstractions;

namespace Open.MessageBroker.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
    where TIntegrationEvent : IntegrationEvent
{
    Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken = default);
    
    Task IIntegrationEventHandler.HandleAsync(IntegrationEvent @event, CancellationToken cancellationToken) 
        => HandleAsync((TIntegrationEvent)@event, cancellationToken);
}


public interface IIntegrationEventHandler
{
    Task HandleAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}
