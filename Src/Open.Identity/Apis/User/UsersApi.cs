using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Open.Core.Results;
using Open.Identity.Apis.User;

namespace Open.Identity.Apis;

public static class UsersApi
{
    public static RouteGroupBuilder MapUsersApiVersionOne(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/users");

        api.MapGet("/", GetAllUserAsync);
        api.MapGet("/{id:guid}", () => { });
        api.MapPost("/", () => { });
        api.MapPut("/{id:guid}", () => { });
        api.MapDelete("/{id:guid}", () => { });

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
