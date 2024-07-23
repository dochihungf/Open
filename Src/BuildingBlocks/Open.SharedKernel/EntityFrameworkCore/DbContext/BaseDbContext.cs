using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Open.Security.Auth;
using Open.SharedKernel.Domain.Entities.Interfaces;
using Open.SharedKernel.Libraries.Helpers;

namespace Open.SharedKernel.EntityFrameworkCore.DbContext;

public class BaseDbContext(DbContextOptions options) : Microsoft.EntityFrameworkCore.DbContext(options), IBaseDbContext
{
    public new virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is null)
        {
            return;
        }
        
        await Database.RollbackTransactionAsync(cancellationToken);
    }
        

    public virtual void BeginTransaction()
    {
        if (Database.CurrentTransaction is null)
        {
            Database.BeginTransaction();
        }
    }
        
    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
            
        if (Database.CurrentTransaction is not null)
        {
            await Database.CommitTransactionAsync(cancellationToken);
        }
    }
    
    protected virtual void ApplyAuditFieldsToModifiedEntities()
    {
        var currentUser = this.GetService<ICurrentUser>();
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added 
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted);

        foreach (var entry in modified)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    if (entry.Entity is IUserTracking userTracking)
                    {
                        userTracking.CreatedBy = currentUser.Context.OwnerId;
                    }

                    if (entry.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.CreatedDate = DateHelper.Now;
                    }
                    break;
                }
                case EntityState.Modified: 
                {
                    Entry(entry.Entity).Property("Id").IsModified = false;
                    if (entry.Entity is IUserTracking userTracking)
                    {
                        userTracking.LastModifiedBy = currentUser.Context.OwnerId;
                    }

                    if (entry.Entity is IDateTracking dateTracking)
                    {
                        dateTracking.LastModifiedDate = DateHelper.Now;
                    }
                    break;
                }
                case EntityState.Deleted:
                {
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        Entry(entry.Entity).Property("Id").IsModified = false;
                        softDelete.DeletedBy = currentUser.Context.OwnerId;
                        softDelete.DeletedDate = DateHelper.Now;
                        softDelete.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }
                    break;
                }
            }
        }
        
    }
}