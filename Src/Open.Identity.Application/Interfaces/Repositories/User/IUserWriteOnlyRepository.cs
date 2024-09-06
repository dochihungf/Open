using Open.Core.Repositories.Dapper;
using Open.Identity.Domain.Entities;

namespace Open.Identity.Application.Interfaces.Repositories;

public interface IUserWriteOnlyRepository : IWriteOnlyRepository<User>
{
}
