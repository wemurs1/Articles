using System.Reflection;
using System.Text.Json.Serialization;
using ArticleHub.Persistence;
using Articles.Security;
using Blocks.Core.Extensions;
using Blocks.Core.Mapster;
using Blocks.Messaging;
using Blocks.Messaging.MassTransit;
using Carter;
using Microsoft.AspNetCore.Http.Json;

namespace ArticleHub.API;

public static class DependancyInjection
{
    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAndValidateOptions<RabbitMqOptions>(configuration)
            .AddAndValidateOptions<HasuraOptions>(configuration)
            .Configure<JsonOptions>(opt =>
            {
                opt.SerializerOptions.PropertyNameCaseInsensitive = true;
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }

    public static IServiceCollection AddApiAndApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services
           .AddCarter()
           .AddHttpContextAccessor()
           .AddEndpointsApiExplorer()
           .AddSwaggerGen()
           .AddJwtAuthentication(config)
           .AddAuthorization();

        services
            .AddMemoryCache()
            .AddMapsterConfigsFromCurrentAssembly()
            .AddMassTransitWithRabbitMQ(config, Assembly.GetExecutingAssembly());

        return services;
    }
}
