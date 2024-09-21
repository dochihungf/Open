﻿using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Open.Core.Repositories.EntityFrameworkCore;
using Open.Core.SeedWork;

namespace Open.SharedKernel.Repositories.EntityFrameworkCore;

public class ReadOnlyRepository<T, TContext>(TContext dbContext) : RepositoryBase<T>(dbContext), IReadRepository<T>
    where T : Entity
    where TContext : DbContext;
