using Moq;
using Moq.Protected;
using Open.UnitTesting.Mocking.Units;

namespace Open.UnitTesting.Mocking;

public class IntranetCommunicationsTests
{
    public class MockHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage()
            {
                Content = new StringContent("[\"Đỗ Chí Hùng\", " +
                                            "\"Đỗ Chí Hoà\", " +
                                            "\"Đinh Minh Quyền\"]")
            });
        }
    }

    [Fact]
    public async Task FetchNamesFetchesNames()
    {
        var client = new HttpClient(new MockHttpHandler())
        {
            BaseAddress = new($"htt" + $"p://example.com")
        };

        var iComm = new IntranetCommunications(client);

        var names = (await iComm.FetchNames() ?? Array.Empty<string>()).ToList();
        
        Assert.NotEmpty(names);
        Assert.Equal(3, names.Count);
        Assert.Contains("Đỗ Chí Hùng", names);
        Assert.Contains("Đỗ Chí Hoà", names);
        Assert.Contains("Đinh Minh Quyền", names);
        
    }

    [Fact]
    public async Task FetchNamesFetchesNames_UsingMock()
    {
        // Arrange
        // Tạo mock cho HttpMessageHandler
        // MockBehavior.Default: Khi bạn kiểm tra các hành vi, Moq sẽ cho phép tất cả các phương thức được gọi mà không báo lỗi.
        // MockBehavior.Strict : Moq sẽ chỉ chấp nhận các phương thức và thuộc tính mà bạn đã thiết lập. Nếu có bất kỳ cuộc gọi nào ngoài các thiết lập đó, Moq sẽ ném ra một ngoại lệ.
        // MockBehavior.Loose: Đây là chế độ mặc định khi không chỉ định MockBehavior. Mọi phương thức chưa được thiết lập sẽ trả về giá trị mặc định của kiểu trả về phương thức (null, 0, false, v.v.).
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Default);
        
        handlerMock
            .Protected()// Cho phép bạn truy cập các phương thức protected của HttpMessageHandler.
            .Setup<Task<HttpResponseMessage>>( // Cấu hình phản hồi cho phương thức protected SendAsync.
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() // Cung cấp phản hồi giả cho SendAsync.
            {
                Content = new StringContent("[\"Đỗ Chí Hùng\", \"Đỗ Chí Hoà\", \"Đinh Minh Quyền\"]")
            })
            .Verifiable(); // Đánh dấu thiết lập này để có thể kiểm tra nó trong phần assert.
        
        
        var client = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new System.Uri($"h" + $"ttp://example.com")
        };
        
        var iComm = new IntranetCommunications(client);
        
        var names = (await iComm.FetchNames() ?? Array.Empty<string>()).ToList();

        Assert.NotEmpty(names);
        Assert.Equal(3, names.Count);
        Assert.Contains("Đỗ Chí Hùng", names);
        Assert.Contains("Đỗ Chí Hoà", names);
        Assert.Contains("Đinh Minh Quyền", names);
    }
}