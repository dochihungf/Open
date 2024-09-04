using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Open.Core.UnitOfWork;

namespace Open.Core.EntityFrameworkCore;

public class AppDbContext : DbContext, IAppDbContext
{
    private IDbContextTransaction? _currentTransaction;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;
    
    public IUnitOfWork UnitOfWork => this;
    
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction == null)
            {
                return;
            }
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await base.Database.BeginTransactionAsync();

        return _currentTransaction;
    }
    
    #region Dispose
    public override void Dispose()
    {
        //GC.SuppressFinalize(this);
    }
    #endregion
}
