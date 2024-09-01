using Open.Security.Enums;

namespace Open.Security.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizationAttribute : Attribute
{
    public ActionExponent[] Exponents { get; } = new ActionExponent[] { ActionExponent.View };

    public AuthorizationAttribute(ActionExponent[] exponents)
    {
        Exponents = Exponents.Concat(exponents).ToArray();
    }

    public AuthorizationAttribute()
    {
    }
}
