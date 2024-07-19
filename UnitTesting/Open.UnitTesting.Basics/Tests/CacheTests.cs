using Open.UnitTesting.Basics.Units;

namespace Open.UnitTesting.Basics.Tests;

public class CacheTests
{
    // Cache item trong khoảng thời gian hợp lệ
    [Fact]
    public void CachesItemWithinTimeSpan()
    {
        var cache = new Cache(TimeSpan.FromDays(1));
        cache.Add(new ("Url", "Content", DateTime.Now));

        var contains = cache.Contains("Url");
        
        Assert.True(contains);
    }

    // Kiểm tra item ngoài khoảng thời gian hợp lệ
    [Fact]
    public void Contains_ReturnsFalse_WhenOutsideTimeSpan()
    {
        var cache = new Cache(TimeSpan.FromDays(1));
        cache.Add(new ("Url", "Content", DateTime.Now.AddDays(-2)));

        var contains = cache.Contains("Url");
        
        Assert.False(contains);
    }

    // fun Contains returns false khi không chưa item có Url
    [Fact]
    public void Contains_ReturnsFalse_WhenDoesntContainItem()
    {
        var cache = new Cache(TimeSpan.FromDays(1));

        var contains = cache.Contains("Url");
        
        Assert.False(contains);
    }
}