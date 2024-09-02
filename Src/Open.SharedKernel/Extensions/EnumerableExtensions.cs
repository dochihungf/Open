using Open.Core.GuardClauses;

namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static IEnumerable<List<TSource>> ChunkList<TSource>(this IEnumerable<TSource> source, int size)
    {
        Guard.Against.Null(source, nameof(source));
        Guard.Against.NegativeOrZero(size, nameof(size));

        var list = source.ToList();
        int count = list.Count;
        var result = new List<List<TSource>>();
        for (int i = 0; i < source.Count(); i += size)
        {
            yield return list.Skip(i).Take(size).ToList();
        }
        
    }
}
