using System.Linq.Dynamic.Core;
using System.Text;
using Open.SharedKernel.EntityFrameworkCore.Models;

namespace Open.SharedKernel.EntityFrameworkCore.Dynamic;

public static class QueryableDynamicFilterExtensions
{
    private static readonly string[] Orders = { "asc", "desc" };
    private static readonly string[] Logics = { "and", "or" };
    
    private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
    {
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "is_null", "== null" },
        { "is_not_null", "!= null" },
        { "starts_with", "StartsWith" },
        { "ends_with", "EndsWith" },
        { "contains", "Contains" },
        { "does_not_contain", "Contains" }
    };
    
    public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQuery dynamicQuery)
    {
        if (dynamicQuery.Filter is not null)
        {
            query = Filter(query, dynamicQuery.Filter);
        }

        if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())
        {
            query = Sort(query, dynamicQuery.Sort);
        }
        
        return query;
    }
    
    private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
    {
        IList<Filter> filters = GetAllFilters(filter);
        object?[] values = filters.Select(f => f.Value).ToArray();
        string where = Transform(filter, filters);
        if (!string.IsNullOrEmpty(where))
        {
            queryable = queryable.Where(where, values);
        }
        return queryable;
    }
    
    private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
    {
        if (sort.Any()) return queryable;
        
        foreach (Sort item in sort)
        {
            if (string.IsNullOrEmpty(item.Field))
            {
                throw new ArgumentException("Invalid Field");
            }

            if (string.IsNullOrEmpty(item.Dir) || !Orders.Contains(item.Dir))
            {
                throw new ArgumentException("Invalid Order Type");
            }
        }
        
        string ordering = string.Join(separator: ",", values: sort.Select(s => $"{s.Field} {s.Dir}"));
        return queryable.OrderBy(ordering);
        
    }

    public static IList<Filter> GetAllFilters(Filter filter)
    {
        List<Filter> filters = [];
        GetFilters(filter, filters);
        return filters;
    }
    
    private static void GetFilters(Filter filter, IList<Filter> filters)
    {
        filters.Add(filter);
        if (filter.Filters is not null && filter.Filters.Any())
        {
            foreach (Filter item in filter.Filters)
            {
                GetFilters(item, filters);
            }
        }
    }
    public static string Transform(Filter filter, IList<Filter> filters)
    {
        if (string.IsNullOrEmpty(filter.Field))
        {
            throw new ArgumentException("Invalid Field");
        }

        if (string.IsNullOrEmpty(filter.Operator) || !Operators.ContainsKey(filter.Operator))
        {
            throw new ArgumentException("Invalid Operator");
        }
        
        int index = filters.IndexOf(filter);
        string comparison = Operators[filter.Operator];
        StringBuilder where = new();

        if (!string.IsNullOrEmpty(filter.Value))
        {
            if (filter.Operator == "does_not_contain")
            {
                where.Append($"(!np({filter.Field}).{comparison}(@{index.ToString()}))");
            }
            else if (comparison is "StartsWith" or "EndsWith" or "Contains")
            {
                where.Append($"(np({filter.Field}).{comparison}(@{index.ToString()}))");
            }
            else
            {
                where.Append($"np({filter.Field}) {comparison} @{index.ToString()}");
            }
        }
        else if(filter.Operator is "is_null" or "is_not_null")
        {
            where.Append($"np({filter.Field}) {comparison}");
        }
        
        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
        {
            if (!Logics.Contains(filter.Logic))
            {
                throw new ArgumentException("Invalid Logic");
            }
            return $"{where} {filter.Logic} ({string.Join(separator: $" {filter.Logic} ", value: filter.Filters.Select(f => Transform(f, filters)).ToArray())})";
        }
        
        return where.ToString();
    }
    
}