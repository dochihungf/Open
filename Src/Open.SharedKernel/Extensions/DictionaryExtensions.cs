using System.Reflection;

namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static void RenameDictionaryKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey fromKey, TKey toKey)
    {
        TValue value = dict[fromKey];
        dict.Remove(fromKey);
        dict[toKey] = value;
    }
    
    public static Dictionary<string, object?> ToDictionary(this object obj)
    {
        Dictionary<string, object?> dictionary = new Dictionary<string, object?>();
        IEnumerable<PropertyInfo> properties = obj.GetType().GetProperties().Where(item => item.GetIndexParameters().Length == 0);
        foreach (PropertyInfo prop in properties)
        {
            dictionary.Add(prop.Name, prop.GetValue(obj));
        }
        return dictionary;
    }

    public static Dictionary<TKey, TValue?> ToDictionary<TKey, TValue>(this object obj)
    {
        Dictionary<TKey, TValue?> dictionary = new Dictionary<TKey, TValue?>();
        IEnumerable<PropertyInfo> properties = obj.GetType().GetProperties().Where(item => item.GetIndexParameters().Length == 0);
        foreach (PropertyInfo prop in properties)
        {
            dictionary.Add((TKey)Convert.ChangeType(prop.Name, typeof(TKey)), (TValue)Convert.ChangeType(prop.GetValue(obj), typeof(TValue))!);
        }
        return dictionary;
    }
}
