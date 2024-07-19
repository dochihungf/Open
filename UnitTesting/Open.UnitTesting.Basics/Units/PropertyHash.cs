using System.Security.Cryptography;
using System.Text;

namespace Open.UnitTesting.Basics.Units;

public class PropertyHash
{
    // Different between a and b: https://viblo.asia/p/01-su-khac-nhau-giua-string-va-stringbuilder-GyZJZX9lVjm
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

public class AlgorithmPropertyHash(string algorithm) : PropertyHash, IDisposable
{
    [Obsolete("Obsolete")]
    private readonly HashAlgorithm _algorithm = HashAlgorithm.Create(algorithm) ?? throw new ArgumentException(algorithm);
    
    [Obsolete("Obsolete")]
    public override string Hash<T>(T input, params Func<T, string>[] selectors)
    {
        var seed = base.Hash(input, selectors);
        var seedBytes = Encoding.UTF8.GetBytes(seed);
        var hashBytes = _algorithm.ComputeHash(seedBytes);
        return Convert.ToBase64String(hashBytes);
    }
    
    [Obsolete("Obsolete")]
    public void Dispose() => _algorithm.Dispose();
}