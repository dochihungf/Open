using Open.Core.EntityFrameworkCore;

namespace Open.Identity.Infrastructure.Persistence;

public class ApplicationDbContext : AppDbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
}
