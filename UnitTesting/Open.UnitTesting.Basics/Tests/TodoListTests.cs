using Open.UnitTesting.Basics.Units;

namespace Open.UnitTesting.Basics.Tests;

public class TodoListTests
{
    [Fact]
    public void Add_SavesTodoItem()
    {
        // arrange (sắp xếp)
        var list = new TodoList();

        // act (Thực hiện)
        list.Add(new ("Đỗ Chí Hùng"));
        
        // assert (xác nhận)
        var savedItem = Assert.Single(list.All);
        Assert.NotNull(savedItem);
        Assert.Equal(1, savedItem.Id);
        Assert.Equal("Đỗ Chí Hùng", savedItem.Content);
        Assert.False(savedItem.Complete);
    }

    [Fact]
    public void TodoItemIdIncrementsEverTimeWeAdd()
    {
        // Arrange
        var list = new TodoList();

        // Act
        list.Add(new ("Test 1"));
        list.Add(new ("Test 2"));
        list.Add(new ("Test 3"));

        // Assert 
        var items = list.All.ToArray();
        
        Assert.Equal(1, items[0].Id);
        Assert.Equal(2, items[1].Id);
        Assert.Equal(3, items[2].Id);
    }

    [Fact]
    public void Complete_SetsTodoItemCompleteFlagToTrue()
    {
        // Arrange
        var list = new TodoList();

        // Act
        list.Add(new ("Test 1"));
        list.Complete(1);

        // Assert 
        var savedItem = Assert.Single(list.All);
        Assert.NotNull(savedItem);
        Assert.Equal(1, savedItem.Id);
        Assert.Equal("Test 1", savedItem.Content);
        Assert.True(savedItem.Complete);
    }
}