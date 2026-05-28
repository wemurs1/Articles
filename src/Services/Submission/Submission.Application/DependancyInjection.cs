using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blocks.MediatR.Behaviors;
using System.Reflection;
using FluentValidation;
using Submission.Application.Features.CreateArticle;

namespace Submission.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddValidatorsFromAssemblyContaining<CreateArticleCommandValidator>()
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(SetUserIdBehavior<,>));
            });

        return services;
    }
}
