using Ardalis.Specification;
using Open.Core.SeedWork.Interfaces;

namespace Open.Core.Repositories.EntityFrameworkCore;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T :  class, IEntity;
