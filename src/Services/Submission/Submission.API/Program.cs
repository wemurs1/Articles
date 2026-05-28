using Submission.API;
using Submission.API.Endpoints;
using Submission.Application;
using Submission.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region Add Services
builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddPersisenceServices(builder.Configuration);
#endregion Add Services

var app = builder.Build();

#region Use Services
app
    .UseSwagger()
    .UseSwaggerUI()
    .UseRouting();

app.MapAllEndpoints();

if (app.Environment.IsDevelopment()) { }
#endregion Use Services

app.Run();
