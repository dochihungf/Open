using Microsoft.EntityFrameworkCore.Storage;

namespace Open.Core.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction?> BeginTransactionAsync();
}
