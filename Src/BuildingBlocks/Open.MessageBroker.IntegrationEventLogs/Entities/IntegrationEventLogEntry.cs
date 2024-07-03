using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Open.MessageBroker.Abstractions.Events;
using Open.MessageBroker.IntegrationEventLogs.Enums;

namespace Open.MessageBroker.IntegrationEventLogs.Entities;

public class IntegrationEventLogEntry
{
    private static readonly JsonSerializerOptions SIndentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions SCaseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    private IntegrationEventLogEntry() { }
    
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
    public Guid EventId { get; private set; }
    
    [Required] public string EventTypeName { get; private set; }
    
    [NotMapped] public string EventTypeShortName => EventTypeName.Split('.')?.Last();
    
    [NotMapped] public IntegrationEvent IntegrationEvent { get; private set; }
    
    public EventStateEnum State { get; set; }
    
    public int TimesSent { get; set; }
    
    public DateTime CreationTime { get; private set; }
    
    [Required] public string Content { get; private set; }
    
    public Guid TransactionId { get; private set; }

    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, SCaseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}