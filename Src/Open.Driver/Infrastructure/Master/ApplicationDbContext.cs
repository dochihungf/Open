using Microsoft.EntityFrameworkCore;
using Open.Core.EntityFrameworkCore;

namespace Open.Driver.Infrastructure.Master;

public class ApplicationDbContext : AppDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}
