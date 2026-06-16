using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blocks.MediatR.Behaviors;
using System.Reflection;
using Submission.Application.Features.CreateArticle;
using Blocks.Core.Mapster;
using Blocks.Messaging.MassTransit;

namespace Submission.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddMapsterConfigsFromCurrentAssembly()
            .AddValidatorsFromAssemblyContaining<CreateArticleCommandValidator>()
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(SetUserIdBehavior<,>));
            })
            .AddMassTransitWithRabbitMQ(config, Assembly.GetExecutingAssembly());

        return services;
    }
}
