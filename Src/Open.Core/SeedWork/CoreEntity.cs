using System.ComponentModel.DataAnnotations.Schema;
using Open.Core.SeedWork.Interfaces;

namespace Open.Core.SeedWork;

public abstract class CoreEntity : ICoreEntity
{
    public string GetTableName()
    {
        if (GetType().IsDefined(typeof(TableAttribute), false))
        {
            return ((TableAttribute)GetType().GetCustomAttributes(typeof(TableAttribute), false).First()).Name;
        }
        return GetType().Name;
    }

    public object? this[string propertyName]
    {
        get
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop == null)
                throw new Exception($"Property {propertyName} does not exists in {GetType().Name}");
            return prop.GetValue(this);
        }

        set
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop == null)
                throw new Exception($"Property {propertyName} does not exists in {GetType().Name}");
            prop.SetValue(this, value);
        }
    }
}
