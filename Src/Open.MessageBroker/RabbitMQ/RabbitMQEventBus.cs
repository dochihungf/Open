using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Open.MessageBroker.Abstractions;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Open.MessageBroker.RabbitMQ;

public sealed class RabbitMQEventBus : IEventBus, IDisposable, IHostedService
{
    private const string ExchangeName = "open_event_bus";

    private readonly ILogger<RabbitMQEventBus> _logger;
    private readonly IServiceProvider _provider;
    private readonly ResiliencePipeline _pipeline;
    private readonly string _queueName;
    private readonly EventBusSubscriptionInfo _subscriptionInfo;
    private IConnection _rabbitMqConnection;
    private IModel _consumerChannel;
    
    public RabbitMQEventBus(
        ILogger<RabbitMQEventBus> logger,
        IServiceProvider provider,
        IOptions<RabbitMQClientSettings> options,
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
        var routingKey = @event.GetType().Name;
        
        using var channel = _rabbitMqConnection.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not open ...");
        channel.ExchangeDeclare(exchange: ExchangeName, type: "direct");
        var body = SerializeMessage(@event);
        
        return _pipeline.Execute(() =>
        {
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            try
            {
                channel.BasicPublish(
                    exchange: ExchangeName,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        });
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Messaging is async so we don't need to wait for it to complete. On top of this
        // the APIs are blocking, so we need to run this on a background thread.
        _ = Task.Factory.StartNew(() =>
        {
            try
            {
                _logger.LogInformation("Starting RabbitMQ connection on a background thread");
                
                _rabbitMqConnection = _provider.GetRequiredService<IConnection>();
                if (!_rabbitMqConnection.IsOpen)
                {
                    return;
                }
        
                _consumerChannel = _rabbitMqConnection.CreateModel();
        
                _consumerChannel.CallbackException += (sender, ea) =>
                {
                    _logger.LogWarning(ea.Exception, "Error with RabbitMQ consumer channel");
                };
        
                _consumerChannel.ExchangeDeclare(exchange: ExchangeName, type: "direct");

                _consumerChannel.QueueDeclare(queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
        
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
        
                consumer.Received += OnMessageReceivedAsync;
        
                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
                
                foreach (var (eventName, _) in _subscriptionInfo.EventTypes)
                {
                    _consumerChannel.QueueBind(
                        queue: _queueName,
                        exchange: ExchangeName,
                        routingKey: eventName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting RabbitMQ connection");
            }
        }, TaskCreationOptions.LongRunning);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        
        try
        {
            await ProcessEventAsync(eventName, message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error Processing message \"{Message}\"", message);
        }
        
        // Even on exception we take the message off the queue.
        // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
        // For more information see: https://www.rabbitmq.com/dlx.html
        _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
    }

    private async Task ProcessEventAsync(string eventName, string message, CancellationToken cancellationToken = default)
    {
        await using var scope = _provider.CreateAsyncScope();
        
        if (!_subscriptionInfo.EventTypes.TryGetValue(eventName, out var eventType))
        {
            _logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }
        
        // Deserialize the event
        var integrationEvent = DeserializeMessage(message, eventType);
        if(integrationEvent == null) return;
        
        // REVIEW: This could be done in parallel

        // Get all the handlers using the event type as the key
        var handlers = scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType);
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(integrationEvent, cancellationToken);
        }
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
        // _consumerChannel?.Dispose();
    }

    #endregion
}
