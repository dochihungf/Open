using Open.Identity.Apis;
using Open.Identity.Infrastructure;
using Open.ServiceDefaults;
using Open.SharedKernel.Settings;

var builder = WebApplication.CreateBuilder(args);

CoreSettings.SetConnectionStrings(builder.Configuration);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var withApiVersioning = builder.Services.AddApiVersioning();
withApiVersioning.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var users = app.NewVersionedApi("Users");

users.MapUsersApiVersionOne();

app.Run();
