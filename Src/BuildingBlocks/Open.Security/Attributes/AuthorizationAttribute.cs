namespace Open.Security.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
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

public enum ActionExponent : int
{
    AllowAnonymous = -1,
    SupperAdmin = 128,
    Admin = 64,
    View = 0,
    Add = 1,
    Edit = 2,
    Delete = 3,
    Export = 4,
    Import = 5,
    Upload = 6,
    Download = 7,
    Update = 8,
}