using Articles.GrpcContracts.Journals;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.Core.Extensions;
using FileStorage.MongoGridFS;

namespace Submission.API;

public static class DependancyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMemoryCache()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        services.AddMongoFileStorage(configuration);

        var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
        services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");
        services.AddCodeFirstGrpcClient<IJournalService>(grpcOptions, "Journal");

        return services;
    }
}
