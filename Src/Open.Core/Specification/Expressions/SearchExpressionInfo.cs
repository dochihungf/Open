using System.Linq.Expressions;

namespace Open.Core.Specification.Expressions;

public class SearchExpressionInfo<T>
{
    private readonly Lazy<Func<T, string>> _selectorFunc;

    public SearchExpressionInfo(Expression<Func<T, string>> selector, string searchTerm, int searchGroup = 1)
    {
        _ = selector ?? throw new ArgumentNullException(nameof(selector));
        if (string.IsNullOrEmpty(searchTerm)) throw new ArgumentException("The search term can not be null or empty.");

        Selector = selector;
        SearchTerm = searchTerm;
        SearchGroup = searchGroup;

        _selectorFunc = new Lazy<Func<T, string>>(Selector.Compile);
    }
    
    /// <summary>
    /// The property to apply the SQL LIKE against.
    /// </summary>
    public Expression<Func<T, string>> Selector { get; }

    /// <summary>
    /// The value to use for the SQL LIKE.
    /// </summary>
    public string SearchTerm { get; }

    /// <summary>
    /// The index used to group sets of Selectors and SearchTerms together.
    /// </summary>
    public int SearchGroup { get; }

    /// <summary>
    /// Compiled <see cref="Selector" />.
    /// </summary>
    public Func<T, string> SelectorFunc => _selectorFunc.Value;
}
