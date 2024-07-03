using System.Net.Sockets;
using Aspire.RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Open.MessageBroker.Abstractions.Abstractions;
using Open.MessageBroker.RabbitMQ.EventBus;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Open.MessageBroker.RabbitMQ;

public static class RabbitMqDependencyInjection
{
    public static IEventBusBuilder AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var configSection = configuration.GetSection(nameof(RabbitMQClientSettings));
        var settings = new RabbitMQClientSettings();
        configSection.Bind(settings);
        
        IConnectionFactory CreateConnectionFactory(IServiceProvider sp)
        {
            var factory = new ConnectionFactory();
            
            // the connection string from settings should win over the one from the ConnectionFactory section
            var connectionString = settings.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                factory.Uri = new(connectionString);
            }

            factory.DispatchConsumersAsync = true;

            return factory;
        }
        
        services.AddSingleton<IConnectionFactory>(CreateConnectionFactory);
        services.AddSingleton<IConnection>(sp => CreateConnection(sp.GetRequiredService<IConnectionFactory>(), settings.MaxConnectRetryCount));
        
        // Options support
        services.Configure<RabbitMQClientSettings>(configSection);

        // Abstractions on top of the core client API
        services.AddSingleton<IEventBus, RabbitMqEventBus>();
        // Start consuming messages as soon as the application starts
        services.AddSingleton<IHostedService>(sp => (RabbitMqEventBus)sp.GetRequiredService<IEventBus>());

        return new EventBusBuilder(services);
    }
    
    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
    
    private static IConnection CreateConnection(IConnectionFactory factory, int retryCount)
    {
        var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
        if (retryCount > 0)
        {
            var options = new RetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = retryCount,
                Delay = TimeSpan.FromSeconds(1),
            };
            options.ShouldHandle = static args => args.Outcome is { Exception: SocketException or BrokerUnreachableException }
                ? PredicateResult.True()
                : PredicateResult.False();
            resiliencePipelineBuilder.AddRetry(options);
        }
        var resiliencePipeline = resiliencePipelineBuilder.Build();
        

        return resiliencePipeline.Execute(static factory =>  factory.CreateConnection(), factory);
    }
}