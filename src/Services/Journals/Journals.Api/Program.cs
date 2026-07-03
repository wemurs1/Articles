using Blocks.FastEndpoints;
using FastEndpoints;
using FastEndpoints.Swagger;
using Journals.Api;
using Journals.Persistence;

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
    .UseFastEndpoints(config =>
    {
        config.Endpoints.Configurator = endpointDefintion =>
        {
            endpointDefintion.PreProcessor<AssignUserIdPreProcessor>(Order.Before);
        };
    })
    .UseSwaggerGen();

if (app.Environment.IsDevelopment()) { }
#endregion Use Services

app.Run();