using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Open.Core.Repositories.EntityFrameworkCore;
using Open.Core.SeedWork;

namespace Open.SharedKernel.Repositories.EntityFrameworkCore;

public class WriteOnlyRepository<T, TContext>(TContext dbContext) : RepositoryBase<T>(dbContext), IWriteRepository<T>
    where T : Entity
    where TContext : DbContext;
