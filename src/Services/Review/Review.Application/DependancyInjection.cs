using System.Reflection;
using Blocks.Core.Mapster;
using Blocks.MediatR.Behaviors;
using Blocks.Messaging.MassTransit;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Review.Application.Features.Invitations.InviteReviewer;

namespace Review.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMapsterConfigsFromCurrentAssembly()
            .AddValidatorsFromAssemblyContaining<InviteReviewerCommandValidator>()
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(AssignUserIdBehaviour<,>));
            })
            .AddMassTransitWithRabbitMQ(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
