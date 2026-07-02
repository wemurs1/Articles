using System.Text.Json;
using System.Text.Json.Serialization;
using Blocks.Core.Extensions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleHub.Persistence;

public static class DependancyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ArticleHubDbContext>(options => options.UseNpgsql(config.GetConnectionString("Database")));

        var hasuraOptions = config.GetSectionByTypeName<HasuraOptions>();

        services.AddSingleton(_ =>
        {
            var graphQlClientOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(hasuraOptions.Endpoint)
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var garphQlClient = new GraphQLHttpClient(graphQlClientOptions, new SystemTextJsonSerializer(jsonSerializerOptions));

            garphQlClient.HttpClient.DefaultRequestHeaders.Add("x-hasura-admin-secret", hasuraOptions.AdminSecret);

            return garphQlClient;
        });

        services.AddScoped<ArticleGraphQLReadStore>();

        return services;
    }
}
