using System.Net.Sockets;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Open.MessageBroker.Abstractions.Abstractions;
using Open.MessageBroker.Abstractions.Events;
using Open.MessageBroker.RabbitMQ.Settings;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Open.MessageBroker.RabbitMQ;

public class RabbitMqEventBus : IEventBus, IDisposable, IHostedService
{
    private const string ExchangeName = "open_event_bus";

    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly IServiceProvider _provider;
    private readonly ResiliencePipeline _pipeline;
    private readonly string _queueName;
    private readonly EventBusSubscriptionInfo _subscriptionInfo;
    private IConnection _rabbitMqConnection;
    private IModel _consumerChannel;

    public RabbitMqEventBus(ILogger<RabbitMqEventBus> logger,
        IServiceProvider provider,
        IOptions<RabbitMqClientSettings> options,
        IOptions<EventBusSubscriptionInfo> subscriptionOptions)
    {
        _logger = logger;
        _provider = provider;
        
        _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
        _queueName = options.Value.SubscriptionClientName;
        _subscriptionInfo = subscriptionOptions.Value;
    }
    
    public Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    
    #region Deserialize and Serialize
    private IntegrationEvent? DeserializeMessage(string message, Type eventType)
    {
        return JsonSerializer.Deserialize(message, eventType, _subscriptionInfo.JsonSerializerOptions) as IntegrationEvent ?? null;
    }

    private byte[] SerializeMessage(IntegrationEvent @event)
    {
        return JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), _subscriptionInfo.JsonSerializerOptions);
    }
    
    #endregion

    #region Polly

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        // See https://www.pollydocs.org/strategies/retry.html
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }

    #endregion
    
    #region Dispose

    public void Dispose()
    {
        _consumerChannel?.Dispose();
    }

    #endregion
}