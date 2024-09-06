using Open.Core.Models;
using Open.Core.Results;
using Open.Identity.Application.DTOs;
using Open.Identity.Application.Interfaces.Repositories;
using Open.Identity.Domain.Entities;
using Open.Security.Auth;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.MySQL;
using Open.SharedKernel.Repositories.Dapper;

namespace Open.Identity.Infrastructure.Repositories;

public class UserReadOnlyRepository : ReadOnlyRepository<User>, IUserReadOnlyRepository
{
    public UserReadOnlyRepository(IDbConnection connection, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider)
        : base(connection, currentUser, sequenceCaching, provider)
    {
    }
}
