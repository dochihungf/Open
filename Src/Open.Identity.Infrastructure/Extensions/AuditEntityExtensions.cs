using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Open.Core.SeedWork.Interfaces;

namespace Open.Identity.Infrastructure.Extensions;

public static class AuditEntityExtensions
{
    public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
    {
        SetSoftDeleteFilterMethod.MakeGenericMethod(entityType).Invoke(null, new object[] { modelBuilder });
    }
    
    static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(AuditEntityExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(t => t.IsGenericMethod && t.Name == nameof(SetSoftDeleteFilter));

    public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder) where TEntity : class, ISoftDelete
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
    }
}
