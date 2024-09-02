using System.Reflection;
using AutoMapper.Configuration.Annotations;

namespace Open.SharedKernel.Extensions;

static partial class Extensions
{
    public static PropertyInfo HasAttribute<TEntity>(this TEntity entity, Type attribute)
    {
        return entity.GetType().GetProperties().FirstOrDefault(prop => prop.IsDefined(attribute, true));
    }

    public static IEnumerable<PropertyInfo> GetPropertyInfos<TEntity>(this TEntity entity)
    {
        return typeof(TEntity)
            .GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0 && !p.IsDefined(typeof(IgnoreAttribute), true));
    }
}
