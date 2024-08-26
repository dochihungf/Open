namespace Open.Core.GuardClauses;

/// <summary>
/// Simple interface to provide a generic mechanism to build guard clause extension methods from.
/// </summary>
public interface IGuardClause
{
    
}

public class Guard : IGuardClause
{
    /// <summary>
    /// An entry point to a set of Guard Clauses.
    /// </summary>
    public static IGuardClause Against { get; } = new Guard();

    private Guard() { }
}
