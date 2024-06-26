namespace Open.MessageBroker.RabbitMQ.Settings;

public sealed class RabbitMqClientSettings
{
    public string? ConnectionString { get; set; }

    public int MaxConnectRetryCount { get; set; } = 5;
    
    public int RetryCount { get; set; } = 10;
    
    public string SubscriptionClientName { get; set; }
}