namespace Open.SharedKernel.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FilterableAttribute(string displayName) : Attribute
{
    public readonly string DisplayName = displayName;
}
