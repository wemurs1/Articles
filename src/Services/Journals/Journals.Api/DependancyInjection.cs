using System.IO.Compression;
using System.Text.Json.Serialization;
using Articles.Security;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.Core.Extensions;
using Blocks.Core.Mapster;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf.Grpc.Server;

namespace Journals.Api;

public static class DependancyInjection
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAndValidateOptions<JwtOptions>(configuration)
            .Configure<JsonOptions>(opt =>
            {
                opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFastEndpoints()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddJwtAuthentication(configuration)
            .AddMapsterConfigsFromCurrentAssembly()
            .AddAuthorization();


        // server
        services.AddCodeFirstGrpc(options =>
        {
            options.ResponseCompressionLevel = CompressionLevel.Fastest;
            options.EnableDetailedErrors = true;
        });

        //client
        var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
        services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");

        return services;
    }
}
