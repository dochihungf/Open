using Open.MessageBroker.Abstractions;

namespace Open.MessageBroker.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}
