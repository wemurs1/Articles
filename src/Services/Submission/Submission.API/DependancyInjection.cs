namespace Submission.API;

public static class DependancyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMemoryCache()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
}
