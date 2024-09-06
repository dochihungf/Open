using Microsoft.Extensions.Localization;
using Open.Identity.Application.Interfaces.Repositories;
using Open.Identity.Domain.Entities;
using Open.Security.Auth;
using Open.SharedKernel.Caching.Sequence;
using Open.SharedKernel.MySQL;
using Open.SharedKernel.Properties;
using Open.SharedKernel.Repositories.Dapper;

namespace Open.Identity.Infrastructure.Repositories;

public class UserWriteOnlyRepository : WriteOnlyRepository<User>, IUserWriteOnlyRepository
{
    public UserWriteOnlyRepository(IDbConnection connection, 
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching, 
        IStringLocalizer<Resources> localizer) 
        : base(connection, currentUser, sequenceCaching, localizer)
    {
    }
}
