using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Open.MessageBroker.IntegrationEventLogs.Entities;

namespace Open.MessageBroker.IntegrationEventLogs.Extensions;

public static class IntegrationLogExtensions
{
    public static void UseIntegrationEventLogs(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntegrationEventLogEntry>(builder =>
        {
            builder.ToTable("integration_event_log");

            builder.HasKey(e => e.EventId);
        });
    }
}