using System.Text.Json.Serialization;
using Articles.Security;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.Core.Extensions;
using Blocks.Messaging;
using Carter;
using EmailService.Smtp;
using FileStorage.MongoGridFS;
using Microsoft.AspNetCore.Http.Json;
using Review.API.FileStorage;

namespace Review.API;

public static class DependancyInjection
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAndValidateOptions<RabbitMqOptions>(configuration)
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
            .AddCarter()
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddJwtAuthentication(configuration)
            .AddAuthorization();

        services.AddMongoFileStorageAsSingleton(configuration);
        services.AddMongoFileStorageAsScoped<SubmissionFileStorageOptions>(configuration);
        services.AddFileServiceFactory();

        services.AddSmtpEmailService(configuration);

        var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
        services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");

        return services;
    }
}
