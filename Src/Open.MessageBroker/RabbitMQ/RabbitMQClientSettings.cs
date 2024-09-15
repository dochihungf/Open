namespace Open.MessageBroker.RabbitMQ;

public sealed class RabbitMQClientSettings
{
    /// <summary>
    /// Gets or sets the connection string of the RabbitMQ server to connect to.
    /// </summary>
    public string ConnectionString { get; set; }
    
    public int ConnectionRetryCount { get; set; } = 5;
    
    public int RetryCount { get; set; } = 10;
    
    public string SubscriptionClientName { get; set; } = "Default";
}
