using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Open.Core.UnitOfWork;

namespace Open.Core.EntityFrameworkCore;

public class AppDbContext : DbContext, IAppDbContext
{
    protected IDbContextTransaction? CurrentTransaction;
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public IDbContextTransaction? GetCurrentTransaction() => CurrentTransaction;
    
    public IUnitOfWork UnitOfWork => this;

    #region UnitOfWork

    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (CurrentTransaction != null)
            {
                await CurrentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (CurrentTransaction == null)
            {
                return;
            }
            await CurrentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }

    public virtual async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (CurrentTransaction != null) return null;

        CurrentTransaction = await base.Database.BeginTransactionAsync();

        return CurrentTransaction;
    }

    #endregion
    
    #region Dispose
    public override void Dispose()
    {
        //GC.SuppressFinalize(this);
    }
    #endregion
}
