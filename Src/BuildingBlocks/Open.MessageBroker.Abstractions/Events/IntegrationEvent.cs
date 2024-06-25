using System.Text.Json.Serialization;

namespace Open.MessageBroker.Abstractions.Events;

public record IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonInclude]
    public DateTime Timestamp { get; protected set; } = DateTime.Now;
}