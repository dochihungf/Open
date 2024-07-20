namespace Open.UnitTesting.Mocking.Units;

public class CreateSomething(CreateSomething.IStore store)
{
    public CreateSomethingResult Create(Something something)
    {
        if (something is { Name: { Length: > 0 } })
        {
            return new(store.Save(something));
        }
        
        return new(false, "Somethings not valid.");
    }
    
    
    public record CreateSomethingResult(bool Success, string Error = "");
    
    public interface IStore
    {
        bool Save(Something someThing);
    }
    
    public class Something
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}