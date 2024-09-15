using Open.MessageBroker.Abstractions;

namespace Open.MessageBroker.IntegrationEventLogs;

public interface IIntegrationEventLogService
{
    /// <summary>
    /// Retrieve Integration event logs pending to published
    /// </summary>
    /// <param name="transactionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Save Integration event
    /// </summary>
    /// <param name="event"></param>
    /// <param name="transaction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    
    Task SaveEventsAsync(List<IntegrationEvent> @events, IDbContextTransaction transaction, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Mark Integration event as published
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark Integration event as published
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark Integration event as failed
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken = default);
}
