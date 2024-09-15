using Open.MessageBroker.Abstractions;

namespace Open.MessageBroker.IntegrationEventLogs;

public class IntegrationEventLogService<TContext> : IIntegrationEventLogService, IDisposable where TContext : DbContext
{
    private volatile bool _disposedValue;
    private readonly TContext _context;
    private readonly Type[] _eventTypes;

    public IntegrationEventLogService(TContext context)
    {
        _context = context;
        var assemblyString = Assembly.GetEntryAssembly()?.FullName;
        if (assemblyString != null)
        {
            _eventTypes = Assembly.Load(assemblyString)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToArray();
        }
    }
    
    public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        var result = await _context.Set<IntegrationEventLogEntry>()
            .Where(e => e.TransactionId == transactionId && e.State == EventStateEnum.NotPublished)
            .ToListAsync(cancellationToken);

        if (result.Count != 0)
        {
            return result.OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.FirstOrDefault(t => t.Name == e.EventTypeShortName) ?? typeof(IntegrationEventLogEntry)));
        }

        return [];
    }
    

    public async Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await SaveEventsAsync(new List<IntegrationEvent>() { @event }, transaction, cancellationToken);
    }

    public async Task SaveEventsAsync(List<IntegrationEvent> @events, IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        
        var eventLogEntries= @events.Select(@e => new IntegrationEventLogEntry(@e, transaction.TransactionId));
        await _context.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);
        await _context.Set<IntegrationEventLogEntry>().AddRangeAsync(eventLogEntries, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        await UpdateEventStatus(eventId, EventStateEnum.Published, cancellationToken);
    }

    public async Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        await UpdateEventStatus(eventId, EventStateEnum.InProgress, cancellationToken);
    }

    public async Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        await UpdateEventStatus(eventId, EventStateEnum.PublishedFailed, cancellationToken);
    }

    private async Task UpdateEventStatus(Guid eventId, EventStateEnum state, CancellationToken cancellationToken = default)
    {
        var integrationEventLogEntry = await _context.Set<IntegrationEventLogEntry>()
            .SingleAsync(ie => ie.EventId == eventId, cancellationToken);
        
        integrationEventLogEntry.State = state;
        
        if (state == EventStateEnum.InProgress)
            integrationEventLogEntry.TimesSent++;

        await _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
