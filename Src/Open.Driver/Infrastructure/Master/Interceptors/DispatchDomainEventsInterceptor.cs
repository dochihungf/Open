using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Open.Core.EntityFrameworkCore;
using Open.Core.SeedWork.Events;
using Open.Driver.Domain.SeedWork;

namespace Open.Driver.Infrastructure.Master.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
    {
        await DispatchDomainEvents(eventData.Context);
        
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;
        
        var entities = context.ChangeTracker
            .Entries<EntityBase>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();
        
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
