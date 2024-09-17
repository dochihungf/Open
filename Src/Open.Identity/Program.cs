using Open.Identity.Apis;
using Open.Identity.Infrastructure;
using Open.Identity.Middlewares;
using Open.ServiceDefaults;
using Open.SharedKernel.Settings;
using Open.SharedKernel.Versioning;

var builder = WebApplication.CreateBuilder(args);

CoreSettings.SetConnectionStrings(builder.Configuration);

builder.AddServiceDefaults();

var withApiVersioning = builder.Services.AddVersioning();
builder.AddDefaultOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseRouting();

// Add your custom logging middleware here
app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

var users = app.NewVersionedApi("Users");
users.MapUsersApiVersionOne();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
