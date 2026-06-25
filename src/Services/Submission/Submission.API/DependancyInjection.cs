using Articles.GrpcContracts.Journals;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.Core.Extensions;
using Blocks.Messaging;
using FileStorage.MongoGridFS;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

namespace Submission.API;

public static class DependancyInjection
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAndValidateOptions<RabbitMqOptions>(config)
            .AddAndValidateOptions<JwtOptions>(config)
            .Configure<JsonOptions>(opt =>
            {
                opt.SerializerOptions.PropertyNameCaseInsensitive = true;
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMemoryCache()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        services.AddMongoFileStorageAsSingleton(configuration);

        var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
        services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");
        services.AddCodeFirstGrpcClient<IJournalService>(grpcOptions, "Journal");

        return services;
    }
}
