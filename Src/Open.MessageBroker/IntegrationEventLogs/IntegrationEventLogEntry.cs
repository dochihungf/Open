using Open.MessageBroker.Abstractions;

namespace Open.MessageBroker.IntegrationEventLogs;

public class IntegrationEventLogEntry
{
    private static readonly JsonSerializerOptions SIndentedOptions = new()
    {
        WriteIndented = true
    };
    
    private static readonly JsonSerializerOptions SCaseInsensitiveOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };


    private IntegrationEventLogEntry()
    {
        
    }
    
    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.Timestamp;
        EventTypeName = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), SIndentedOptions);
        State = EventStateEnum.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
    }
    
    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, SCaseInsensitiveOptions) as IntegrationEvent 
                           ?? throw new ArgumentNullException(nameof(IntegrationEvent));
        return this;
    }
    
    public Guid EventId { get; private set; }
    
    [Required] 
    public string EventTypeName { get; private set; }
    
    [NotMapped] 
    public string? EventTypeShortName => EventTypeName.Split('.')?.Last();
    
    [NotMapped] 
    public IntegrationEvent IntegrationEvent { get; private set; }
    
    public EventStateEnum State { get; set; }
    
    public int TimesSent { get; set; }
    
    public DateTime CreationTime { get; private set; }
    
    [Required] 
    public string Content { get; private set; }
    
    public Guid TransactionId { get; private set; }
    
}
