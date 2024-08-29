using Open.Core.GuardClauses;

namespace GuardClauses.UnitTests;

public class GuardAgainstNull
{
    /// <summary>
    /// Does nothing given non-null value
    /// Không làm gì khi nhận giá trị không phải null
    /// </summary>
    [Fact]
    public void DoesNothingGivenNonNullValue()
    {
        Guard.Against.Null("", "string");
        Guard.Against.Null(1, "int");
        Guard.Against.Null(new Object(), "object");
        Guard.Against.Null(Guid.Empty, "guid");
        Guard.Against.Null(DateTime.Now, "datetime");
    }

    /// <summary>
    /// Throws given null value
    /// Ném lỗi khi nhận giá trị null
    /// </summary>
    [Fact]
    public void ThrowsGivenNullValue()
    {
        object obj = null!;
        Assert.Throws<ArgumentNullException>(() => Guard.Against.Null(obj, "object"));
    }
    
    /// <summary>
    ///  Throws custom exception when supplied given null value
    /// Ném ngoại lệ tùy chỉnh khi nhận giá trị null
    /// </summary>
    [Fact]
    public void ThrowsGivenNonNullValue()
    {
        object obj = null!;
        Assert.Throws<Exception>(() => Guard.Against.Null(obj, "null", exceptionCreator: () => new Exception()));
    }

    /// <summary>
    /// Returns non nullable value type when given nullable value type is not null
    /// Trả về kiểu giá trị không nullable khi kiểu giá trị nullable cho trước không phải null
    /// </summary>
    [Fact]
    public void ReturnsNonNullableValueTypeWhenGivenNullableValueTypeIsNotNull()
    { 
        int? @int = 4;
        Assert.False(IsNullableType(Guard.Against.Null(@int, "int")));
        
        double? @double = 11.11;
        Assert.False(IsNullableType(Guard.Against.Null(@double, "@double")));

        DateTime? now = DateTime.Now;
        Assert.False(IsNullableType(Guard.Against.Null(now, "now")));

        Guid? guid = Guid.Empty;
        Assert.False(IsNullableType(Guard.Against.Null(guid, "guid")));
        
        object? obj = new object();
        var result = IsNullableType(Guard.Against.Null(obj, "object"));
        Assert.True(result);
        
        static bool IsNullableType<T>(T value) // Là kiểu nullable int?, object?, ...
        {
            if (value is null)
            {
                return false;
            }

            Type type = typeof(T); // type
            // typeof(Object).IsValueType = typeof(ReferenceTypes).IsValueType => false 
            // typeof(int).IsValueType = typeof(ValueTypes).IsValueType => true
            if (!type.IsValueType) // check type is value type
            {
                return true;
            }
            
            // Nullable.GetUnderlyingType(type): Phương thức này trả về kiểu cơ sở của kiểu nullable type.
            // Ví dụ type là int? return typeof(int)
            // type is not nullable return null
            
            if (Nullable.GetUnderlyingType(type) != null)
            {
                return true;
            }
            
            return false;
        }
    }

    /// <summary>
    /// Error message matches expected
    /// Thông báo khớp với mong đợi
    /// </summary>
    [Theory]
    [InlineData(null, "Value cannot be null. (Parameter 'parameterName')")]
    [InlineData("Please provide correct value", "Please provide correct value (Parameter 'parameterName')")]
    public void ErrorMessageMatchesExpected(string? customMessage, string? expectedMessage)
    {
        string? nullString = null;
        var exception = Assert.Throws<ArgumentNullException>(() => Guard.Against.Null(nullString, "parameterName", customMessage));
        Assert.NotNull(exception);
        Assert.NotNull(exception.Message);
        Assert.Equal(expectedMessage, exception.Message);
    }

    /// <summary>
    /// Error message matches expected when name not explicitly provided
    /// Thông báo lỗi khớp với mong đợi khi tên không được cung cấp rõ ràng
    /// </summary>
    /// <returns></returns>
    [Fact]
    public void ErrorMessageMatchesExpectedWhenNameNotExplicitlyProvided()
    {
        string? xyz = null;

        var exception = Assert.Throws<ArgumentNullException>(() => Guard.Against.Null(xyz));

        Assert.NotNull(exception);
        Assert.NotNull(exception.Message);
        Assert.Contains($"Value cannot be null. (Parameter '{nameof(xyz)}')", exception.Message);
    }
    
    /// <summary>
    /// Exception paramName matches expected
    /// Tên tham số của ngoại lệ khớp với mong đợi
    /// </summary>
    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "Please provide correct value")]
    [InlineData("SomeParameter", null)]
    [InlineData("SomeOtherParameter", "Value must be correct")]
    public void ExceptionParamNameMatchesExpected(string? expectedParamName, string? customMessage)
    {
        string? nullString = null;
        var exception = Assert.Throws<ArgumentNullException>(() => Guard.Against.Null(nullString, expectedParamName, customMessage));
        Assert.NotNull(exception);
        Assert.Equal(expectedParamName, exception.ParamName);
    }
}
