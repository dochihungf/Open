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

    #region ExistsAsync

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

    #endregion

    #region SetAsync

    [Fact]
    public async Task SetAsync_WithValidParameters_SetsValueAndReturnsTrue()
    {
        // Arrange
        string key = "testKey";
        var value = new { Name = "Đỗ Chí Hùng", Age = 22 };
        
        // Setup IMemoryCache.Set method
        _mockCache
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>);

        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            It.IsAny<object>(),
            out It.Ref<object>.IsAny!
        )).Returns(true);
        
        // Act
        var result = await _caching.SetAsync(key, value);
        
        // Assert
        Assert.True(result);      
    }
    
    [Fact]
    public async Task SetAsync_WithValidParameters_SetsValueAndReturnsFalse()
    {
        // Arrange
        string key = "testKey";
        var value = new { Name = "Đỗ Chí Hùng", Age = 22 };
        
        // Setup IMemoryCache.Set method
        _mockCache
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>);

        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            It.IsAny<object>(),
            out It.Ref<object>.IsAny!
        )).Returns(false);
        
        // Act
        var result = await _caching.SetAsync(key, value);
        
        // Assert
        Assert.False(result);      
    }

    [Fact]
    public async Task SetAsync_WithNullKey_ThrowsArgumentNullException()
    {
        // Arrange
        string key = string.Empty;
        var value = new { Name = "Đỗ Chí Hùng", Age = 22 };

        // Act
        
        // Setup IMemoryCache.Set method
        _mockCache
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>);;
        
        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            It.IsAny<object>(),
            out It.Ref<object>.IsAny!
        )).Returns(true);

        // Assert
        // Act and Assert
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _caching.SetAsync(key, value));
        Assert.Equal("key", ex.ParamName);
    }

    [Fact]
    public async Task SetAsync_WithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string key = "testKey";
        object value = null!;

        // Setup IMemoryCache.Set method
        _mockCache
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>);
        
        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            It.IsAny<object>(),
            out It.Ref<object>.IsAny!
        )).Returns(true);
        
        // Act
        

        // Assert
        // Act and Assert
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _caching.SetAsync(key, value));
        Assert.Equal("value", ex.ParamName);
    }


    #endregion

    #region GetAsync

    [Fact]
    public async Task GetStringAsync_WithValidKey_ReturnsValue()
    {
        // Arrange
        string key = "testKey";
        object? cachedValue = "testValue";
        
        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            key,
            out cachedValue
        )).Returns(true);

        // Act
        var result = await _caching.GetStringAsync(key);
        
        // Asser
        _mockCache.Verify(c => c.TryGetValue(key, out cachedValue), Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(cachedValue, result);
    }

    [Fact]
    public async Task GetStringAsync_WithInValidKey_ReturnsEmptyString()
    {
        // Arrange
        string key = "testKey";
        object? cachedValue = string.Empty;
        
        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            key,
            out cachedValue
        )).Returns(false);
        
        // Act
        var result = await _caching.GetStringAsync(key);
        
        // Asser
        _mockCache.Verify(c => c.TryGetValue(key, out cachedValue), Times.Once);
        
        Assert.NotNull(result);
        Assert.Equal(cachedValue, result);
        
    }

    [Fact]
    public async Task GetStringAsync_WithNullKey_ThrowsArgumentNullException()
    {
        // Arrange
        string key = string.Empty;
        
        // Assert
        // Act and Assert
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _caching.GetStringAsync(key));
        Assert.Equal("key", ex.ParamName);
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_WithValidKey_ReturnsTrue()
    {
        // Arrange
        string key = "testKey";
        object value = null!;
        
        // Setup IMemoryCache.Remove
        _mockCache
            .Setup(cache => cache.Remove(key))
            .Verifiable();
        
        // Setup IMemoryCache.TryGetValue method
        _mockCache.Setup(cache => cache.TryGetValue(
            It.IsAny<object>(),
            out It.Ref<object>.IsAny!
        )).Returns(false);
        
        // Act
        var result = await _caching.DeleteAsync(key);

        // Assert
        _mockCache.Verify(cache => cache.Remove(key), Times.Once);
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteAsync_WithNullOrEmptyKey_ThrowsArgumentNullException()
    {
        // Arrange
        string key = string.Empty;
        
        // Assert
        // Act and Assert
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _caching.DeleteAsync(key));
        Assert.Equal("key", ex.ParamName);
    }
    
    #endregion
    
    
}