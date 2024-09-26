using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Open.Driver.Domain.SeedWork;
using Open.Security.Auth;
using Open.SharedKernel.Libraries.Helpers;

namespace Open.Driver.Infrastructure.Master.Interceptors;

public class EntityBaseInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _user;
    private readonly TimeProvider _dateTime;

    public EntityBaseInterceptor(ICurrentUser user, TimeProvider dateTime)
    {
        _user = user;
        _dateTime = dateTime;
    }
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if(context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _user.Context.OwnerId;
                    entry.Entity.CreatedDate = DateHelper.Now;
                    break;
                    
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _user.Context.OwnerId;
                    entry.Entity.LastModifiedDate = DateHelper.Now;
                    break;
                
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedBy = _user.Context.OwnerId;
                    entry.Entity.DeletedDate = DateHelper.Now;
                    entry.Entity.Delete();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }
    }

}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
