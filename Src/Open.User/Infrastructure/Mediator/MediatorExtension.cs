using System.Collections.Immutable;
using MediatR;
using Open.User.Domain.SeedWork;
using Open.User.Infrastructure.Master;

namespace Open.User.Infrastructure.Mediator;

internal static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IPublisher publisher, ApplicationDbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<EntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToImmutableList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToImmutableList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
