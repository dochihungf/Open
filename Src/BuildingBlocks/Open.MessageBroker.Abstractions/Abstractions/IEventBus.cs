using Open.MessageBroker.Abstractions.Events;

namespace Open.MessageBroker.Abstractions.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}