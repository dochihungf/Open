using Open.Core.Models;
using Open.Core.Repositories.Dapper;
using Open.Core.Results;
using Open.Identity.Application.DTOs;
using Open.Identity.Domain.Entities;

namespace Open.Identity.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository : IReadOnlyRepository<User>
{
    
}
