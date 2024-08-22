using System.Linq.Expressions;
using Open.Core.Specification.Enums;

namespace Open.Core.Specification.Expressions;

public class IncludeExpressionInfo
{
    public LambdaExpression LambdaExpression { get; }

    /// <summary>
    /// The type of the source entity.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// The type of the included entity.
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// The type of the previously included entity.
    /// </summary>
    public Type? PreviousPropertyType { get; }

    /// <summary>
    /// The include type.
    /// </summary>
    public IncludeTypeEnum Type { get; }

    private IncludeExpressionInfo(LambdaExpression expression,
                                  Type entityType,
                                  Type propertyType,
                                  Type? previousPropertyType,
                                  IncludeTypeEnum includeType)

    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        _ = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

        if (includeType == IncludeTypeEnum.ThenInclude)
        {
            _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));
        }

        LambdaExpression = expression;
        EntityType = entityType;
        PropertyType = propertyType;
        PreviousPropertyType = previousPropertyType;
        Type = includeType;
    }
    
    public IncludeExpressionInfo(LambdaExpression expression,
                                 Type entityType,
                                 Type propertyType)
        : this(expression, entityType, propertyType, null, IncludeTypeEnum.Include)
    {
    }
    
    public IncludeExpressionInfo(LambdaExpression expression,
                                 Type entityType,
                                 Type propertyType,
                                 Type previousPropertyType)
        : this(expression, entityType, propertyType, previousPropertyType, IncludeTypeEnum.ThenInclude)
    {
    }
}
