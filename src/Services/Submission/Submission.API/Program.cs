using Blocks.AspNetCore;
using Blocks.AspNetCore.Filters;
using Submission.API;
using Submission.API.Endpoints;
using Submission.Application;
using Submission.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region Add Services
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
    .UseMiddleware<GlobalExceptionMiddleware>();

app.MapAllEndpoints();
app.MapGroup("/api").AddEndpointFilter<AssignUserIdFilter>();

if (app.Environment.IsDevelopment()) { }
#endregion Use Services

app.Run();
