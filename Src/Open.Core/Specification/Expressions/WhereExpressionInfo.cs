using System.Linq.Expressions;

namespace Open.Core.Specification.Expressions;

public class WhereExpressionInfo<T>
{
    private readonly Lazy<Func<T, bool>> _filterFunc;
    
    public WhereExpressionInfo(Expression<Func<T, bool>> filter)
    {
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        Filter = filter;

        _filterFunc = new Lazy<Func<T, bool>>(Filter.Compile);
    }
    
    /// <summary>
    /// Condition which should be satisfied by instances of <typeparamref name="T"/>.
    /// </summary>
    public Expression<Func<T, bool>> Filter { get; }

    /// <summary>
    /// Compiled <see cref="Filter" />.
    /// </summary>
    public Func<T, bool> FilterFunc => _filterFunc.Value;
}
