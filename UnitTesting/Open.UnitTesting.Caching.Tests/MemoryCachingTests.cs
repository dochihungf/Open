using Microsoft.Extensions.Caching.Memory;
using Moq;
using Open.Core.Caching.InMemory;

namespace Open.UnitTesting.Caching.Tests;

public class MemoryCachingTests
{
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly MemoryCaching _caching;

    public MemoryCachingTests()
    {
        _mockCache = new Mock<IMemoryCache>();
        _caching = new MemoryCaching(_mockCache.Object);
    }
    
    [Fact]
    public async Task ExistsAsync_WithValidKey_ReturnsTrue()
    {
        // Arrange
        string key = "testKey";
        object? cachedValue;
        _mockCache.Setup(c => c.TryGetValue(key, out cachedValue)).Returns(true);

        // Act
        var exists = await _caching.ExistsAsync(key);

        // Assert
        Assert.True(exists);
        _mockCache.Verify(c => c.TryGetValue(key, out cachedValue), Times.Once);
    }

    [Fact]
    public async Task ExistsAsync_WithInValidKey_ReturnsFalse()
    {
        // Arrange
        string key = "testKey";
        object? cachedValue;
        _mockCache.Setup(c => c.TryGetValue(key, out cachedValue)).Returns(false);
        
        // Act
        var exists = await _caching.ExistsAsync(key);

        // Assert
        Assert.False(exists);
        _mockCache.Verify(c => c.TryGetValue(key, out cachedValue), Times.Once);
    }
    
}