namespace Open.Core.Specification.Evaluators;

public interface IEvaluator
{
    bool IsCriteriaEvaluator { get; }
    
    IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
}
