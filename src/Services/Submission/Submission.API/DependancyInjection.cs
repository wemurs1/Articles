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

        return services;
    }
}
