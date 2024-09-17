using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Open.Core.Results;

namespace Open.Identity.Apis;

public static class UsersApi
{
    public static RouteGroupBuilder MapUsersApiVersionOne(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/identity/users").HasApiVersion(1.0);

        app.MapGet("/", GetAllUserAsync);

        return api;
    }

    public static async Task<Results<Ok<Result<List<string>>>, ProblemHttpResult>> GetAllUserAsync()
    {
        var users = new List<string>()
        {
            "a","b","c","d","e","f","g","h","i",
        };
        return TypedResults.Ok(Result.Success(users));
    }
}
