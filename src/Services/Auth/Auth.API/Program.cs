using Auth.API;
using Auth.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApiOptions(builder.Configuration);

#region Add Services
builder.Services
    .AddApiServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);
#endregion Add Services

var app = builder.Build();

#region Use Services
app
    .UseSwagger()
    .UseSwaggerUI()
    .UseRouting()
    .UseFastEndpoints()
    .UseSwaggerGen();

if (app.Environment.IsDevelopment()) { }
#endregion Use Services

app.Run();
