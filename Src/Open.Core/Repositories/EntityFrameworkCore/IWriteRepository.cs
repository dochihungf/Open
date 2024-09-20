using Ardalis.Specification;

namespace Open.Core.Repositories.EntityFrameworkCore;

public interface IWriteRepository<T> : IRepositoryBase<T> where T : class;
