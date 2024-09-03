using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Open.Core.UnitOfWork;

namespace Open.Identity.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
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
                _currentTransaction = default(IDbContextTransaction)!;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction == default!)
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
                _currentTransaction = default!;
            }
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await base.Database.BeginTransactionAsync();

        return _currentTransaction;
    }
    
}
