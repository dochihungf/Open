using Open.MessageBroker.Abstractions.Events;

namespace Open.MessageBroker.Abstractions.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
    where TIntegrationEvent : IntegrationEvent
{
    
}

public interface IIntegrationEventHandler
{
    Task HandleAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}

