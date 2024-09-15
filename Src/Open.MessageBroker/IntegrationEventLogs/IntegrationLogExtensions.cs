namespace Open.MessageBroker.IntegrationEventLogs;

public static class IntegrationLogExtensions
{
    public static void UseIntegrationEventLogs(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntegrationEventLogEntry>(builder =>
        {
            builder.ToTable("integration_event_logs");

            builder.HasKey(e => e.EventId);
        });
    }
}
