using Open.Core.GuardClauses;
using Open.Core.GuardClauses.Exceptions;

namespace GuardClauses.UnitTests;

public class GuardAgainstNotFound
{
    /// <summary>
    /// Does nothing given non-null value
    /// Không làm gì khi nhận giá trị không phải null
    /// </summary>
    [Fact]
    public void DoesNothingGivenNonNullValue()
    {
        Guard.Against.NotFound("key", "", "string");
        Guard.Against.NotFound(1, 1, "int");
        Guard.Against.NotFound(1, Guid.Empty, "guid");
        Guard.Against.NotFound(Guid.Empty, DateTime.Now, "datetime");
        Guard.Against.NotFound(1, new Object(), "object");
    }
    
    /// <summary>
    /// Throws given null value
    /// Ném lỗi khi nhận giá trị null
    /// </summary>
    [Fact]
    public void ThrowsGivenNullValue()
    {
        object obj = null!;
        Assert.Throws<NotFoundException>(() => Guard.Against.NotFound(1, obj, "null"));
    }
    
    /// <summary>
    /// Throws custom exception when supplied given null value
    /// Ném ngoại lệ tùy chỉnh khi nhận giá trị null được cung cấp
    /// </summary>
    [Fact]
    public void ThrowsCustomExceptionWhenSuppliedGivenNullValue()
    {
        object obj = null!;
        Exception customException = new Exception();
        Assert.Throws<Exception>(() => Guard.Against.NotFound(1, obj, "null", exceptionCreator: () => customException));
    }
    
    /// <summary>
    /// Returns expected value when given non null value
    /// Trả về giá trị mong đợi khi nhận giá trị không null
    /// </summary>
    [Fact]
    public void ReturnsExpectedValueWhenGivenNonNullValue()
    {
        Assert.Equal("", Guard.Against.NotFound("mykey", "", "string"));
        Assert.Equal(1, Guard.Against.NotFound(1, 1, "int"));

        var guid = Guid.Empty;
        Assert.Equal(guid, Guard.Against.NotFound(1, guid, "guid"));

        var now = DateTime.Now;
        Assert.Equal(now, Guard.Against.NotFound(1, now, "datetime"));

        var obj = new Object();
        Assert.Equal(obj, Guard.Against.NotFound(1, obj, "object"));
    }

    /// <summary>
    /// Error message matches expected when name not explicitly provided given string value
    /// Thông báo lỗi khớp với mong đợi khi tên không được cung cấp rõ ràng với giá trị chuỗi được cho
    /// </summary>
    [Fact]
    public void ErrorMessageMatchesExpectedWhenNameNotExplicitlyProvidedGivenStringValue()
    {
        string? xyz = null;
        var key = "key";

        var exception = Assert.Throws<NotFoundException>(() => Guard.Against.NotFound(key, xyz));

        Assert.NotNull(exception);
        Assert.NotNull(exception.Message);
        Assert.Contains($"Queried object {nameof(xyz)} was not found, Key: {key}", exception.Message);
    }
    
}
