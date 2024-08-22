namespace Open.Core.Specification;

public interface ISpecification
{
   // ISpecificationBuilder<T> Query { get; }
   
   IDictionary<string, object> Items { get; set; }
   
   
   
}
