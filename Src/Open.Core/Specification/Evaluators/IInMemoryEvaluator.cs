namespace Open.Core.Specification.Evaluators;

public interface IInMemoryEvaluator
{
    IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
}
