using Microsoft.Extensions.DependencyInjection;

namespace Open.MessageBroker.Abstractions.Abstractions;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}