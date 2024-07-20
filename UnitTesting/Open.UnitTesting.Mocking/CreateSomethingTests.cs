using Moq;
using Open.UnitTesting.Mocking.Units;

namespace Open.UnitTesting.Mocking;

public class CreateSomethingTests
{
    public class StoreMock : CreateSomething.IStore
    {
        // Số lần save
        public int SaveAttempts { get; set; }
        public bool SaveResult { get; set; }
        public CreateSomething.Something LastSavedSomething { get; set; }
        public bool Save(CreateSomething.Something something)
        {
            SaveAttempts++;
            LastSavedSomething = something;
            return SaveResult;
        }
    }

    [Fact]
    public void DoesntSaveToDatabaseWhenInvalidSomething()
    {
        var storeMock = new StoreMock();
        CreateSomething createSomething = new(storeMock);

        var createSomethingResult = createSomething.Create(null);
        
        Assert.False(createSomethingResult.Success);
        Assert.Equal(0, storeMock.SaveAttempts);
    }
    
    [Fact]
    public void SavesSomethingToDatabaseWhenValid()
    {
        var storeMock = new StoreMock();
        storeMock.SaveResult = true;
        CreateSomething createSomething = new(storeMock);

        var something = new CreateSomething.Something()
        {
            Name = "Đỗ Chí Hùng",
            Id = 1
        };
        
        var createSomethingResult = createSomething.Create(something);
        
        Assert.True(createSomethingResult.Success);
        Assert.Equal(1, storeMock.SaveAttempts);
        Assert.Equal(something, storeMock.LastSavedSomething);

    }

    public Mock<CreateSomething.IStore> _storeMock = new();

    [Fact]
    public void DoesntSaveToDatabaseWhenInvalidSomething_UsingMock()
    {
        CreateSomething createSomething = new(_storeMock.Object);

        var createSomethingResult = createSomething.Create(null);
        
        Assert.False(createSomethingResult.Success);
        Assert.Equal("Somethings not valid.", createSomethingResult.Error);
        
        _storeMock.Verify(x => x.Save(It.IsAny<CreateSomething.Something>()), Times.Never);
    }
    
    [Fact]
    public void SavesSomethingToDatabaseWhenValid_UsingMock()
    {
        CreateSomething createSomething = new(_storeMock.Object);
        var something = new CreateSomething.Something()
        {
            Name = "Đỗ Chí Hùng",
            Id = 1
        };
        
        _storeMock.Setup(x => x.Save(something)).Verifiable();
        _storeMock.Setup(x => x.Save(something)).Returns(true);
        
        var createSomethingResult = createSomething.Create(something);
        
        Assert.True(createSomethingResult.Success);
        _storeMock.Verify(x => x.Save(something), Times.Once);
    }

    /* Times của thư viện Moq, và nó được sử dụng trong phương thức Verify
    /// Times.Never: không được gọi bất kỳ lần nào
    /// Times.Once: Phương thức phải được gọi chính xác một lần.
    /// Times.AtLeastOnce: Phương thức phải được gọi ít nhất một lần.
    /// Times.Exactly(n): Phương thức phải được gọi đúng số lần n.
    /// Times.AtLeast(n): Phương thức phải được gọi ít nhất n lần.
    /// Times.AtMost(n): Phương thức không được gọi quá n lần.
    */ 
}