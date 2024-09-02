namespace Open.SharedKernel.Libraries.Utilities;

public static class Utilities
{
    public static string RandomString(int length, bool hasNumber = true)
    {
        var random = new Random();
        var mix = Enumerable.Range(65, 26).Concat(Enumerable.Range(97, 26)).ToList();
        if (hasNumber)
        {
            mix.Concat(Enumerable.Range(48, 10));
        }

        var result = new List<char>();
        var mixCount = mix.Count;
        if (length <= mixCount)
        {
            return string.Join("", mix.OrderBy(x => random.Next()).Take(length).Select(x => (char)x));
        }

        while (length > 0)
        {
            result.AddRange(mix.OrderBy(x => random.Next()).Take(length).Select(x => (char)x));
            length -= mixCount;
        }

        return string.Join("", result);
    }

}
