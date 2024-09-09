namespace Open.ServiceDefaults;

public static class CorsExtensions
{
    public static void UseCoreCors(this IApplicationBuilder app, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredSection("Allowedhosts").Value;
        if (origins.Equals("*"))
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
        else
        {
            app.UseCors(x => x.WithOrigins(origins.Split(";")).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
        }
    }
}
