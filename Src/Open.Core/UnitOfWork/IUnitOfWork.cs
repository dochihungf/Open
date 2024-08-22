namespace Open.Core.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    
    Task CommitAsync(CancellationToken cancellationToken = new CancellationToken());

    Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken());

    void BeginTransaction();
}
