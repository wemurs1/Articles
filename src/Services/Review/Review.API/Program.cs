using Carter;
using Review.API;
using Review.Application;
using Review.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region Add Services
builder.Services
    .ConfigureApiOptions(builder.Configuration);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);
#endregion Add Services

var app = builder.Build();

#region Use Services
app
    .UseSwagger()
    .UseSwaggerUI()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapCarter();

if (app.Environment.IsDevelopment()) { }
#endregion Use Services

app.Run();
