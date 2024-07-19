using System.Security.Cryptography;
using System.Text;

namespace Open.UnitTesting.Mocking.Units;

public class PropertyHash
{
    public virtual string Hash<T>(T input, params Func<T, string>[] selectors)
    {
        StringBuilder builder = new();
        foreach (var selector in selectors)
        {
            builder.Append(selector(input));
        }

        return builder.ToString();
    }
}

public interface IHashAlgorithmFactory
{
    public HashAlgorithm Create();
}

public class AlgorithmPropertyHash(IHashAlgorithmFactory algorithmFactory)
{
    public string Hash(string seed)
    {
        var seedBytes = Encoding.UTF8.GetBytes(seed);
        using var algo = algorithmFactory.Create();
        var hashBytes = algo.ComputeHash(seedBytes);
        return Convert.ToBase64String(hashBytes);
    }
}