namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static IEnumerable<List<TSource>> ChunkList<TSource>(this IEnumerable<TSource> source, int size)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (size < 1)
        {
            throw new ArgumentOutOfRangeException("size", "size must be greater than 0");
        }

        var result = new List<List<TSource>>();
        for (int i = 0; i < source.Count(); i += size)
        {
            //yield return source.ToList().GetRange(i, Math.Min(size, source.Count() - i));
            result.Add(source.ToList().GetRange(i, Math.Min(size, source.Count() - i)));
        }
        return result;
    }
}
