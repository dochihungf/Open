namespace Open.UnitTesting.Basics.Units;

public class TodoList
{
    public record TodoItem(string Content)
    {
        public int Id { get; init; }
        public bool Complete  { get; init; }
    }

    private int _idCounter = 1;
    
    public int IdCounter => _idCounter;
    
    private readonly List<TodoItem> _todoItems = new();
        
    public void Add(TodoItem item)
    {
        _todoItems.Add(item with { Id = _idCounter++ });
    }
        
    public IEnumerable<TodoItem> All => _todoItems;

    public void Complete(int id)
    {
        var item = _todoItems.First(x => x.Id == id);
        _todoItems.Remove(item);

        var completedItem = item with {Complete = true};
        _todoItems.Add(completedItem);
    }
}