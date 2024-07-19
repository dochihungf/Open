using Open.UnitTesting.Basics.Units;

namespace Open.UnitTesting.Basics.Tests;

public class PropertyHashTests
{
    [Fact]
    public void PropertyHash_ConcatenatesSelectedFieldsInOrder()
    {
        var hasher = new PropertyHash();
        var item = new Cache.Item("Url", "Content", DateTime.Now);

        var hash = hasher.Hash(item, i => i.Url, i => i.Content);
        
        Assert.Equal("UrlContent", hash);
    }

    [Fact]
    public void AlgorithmPropertyHash_AppliesHashingAlgorithmToSeed()
    {
        var hasher = new AlgorithmPropertyHash("SHA256");
        var item = new Cache.Item("Url", "Content", DateTime.Now);

        var hash = hasher.Hash(item, i => i.Url, i => i.Content);

        var resultFalse = "".Equals(hash);
        var resultTrue = "sr8K/jFVL+8CNjLQO0gWha4UWUSdSPwywvJfSzgGmic=".Equals(hash);
        Assert.False(resultFalse);
        Assert.True(resultTrue);
    }
}