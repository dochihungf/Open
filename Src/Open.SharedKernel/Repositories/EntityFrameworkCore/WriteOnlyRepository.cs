using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Open.Core.EntityFrameworkCore;
using Open.Core.Repositories.EntityFrameworkCore;
using Open.Core.SeedWork;

namespace Open.SharedKernel.Repositories.EntityFrameworkCore;

public abstract class WriteOnlyRepository<T, TContext> : ReadOnlyRepository<T, TContext>, IWriteRepository<T> where T : class, IEntity
    where TContext : AppDbContext
{
    protected WriteOnlyRepository(TContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }

    protected WriteOnlyRepository(TContext dbContext, 
        SpecificationEvaluator specificationEvaluator, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider) : base(dbContext, 
        specificationEvaluator, 
        currentUser, 
        sequenceCaching, provider)
    {
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public virtual Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public virtual Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().AddRange(entities);
        
        return Task.FromResult(entities);
    }

    /// <inheritdoc/>
    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }
    /// <inheritdoc/>
    public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification);
        _dbContext.Set<T>().RemoveRange(query);
        return Task.CompletedTask;
    }
}
