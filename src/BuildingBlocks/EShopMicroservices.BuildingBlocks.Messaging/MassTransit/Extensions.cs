using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EShopMicroservices.BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config =>
        {
            var hostUrl = configuration["MessageBroker:Host"];
            var userName = configuration["MessageBroker:UserName"];
            var password = configuration["MessageBroker:Password"];

            ArgumentException.ThrowIfNullOrWhiteSpace(hostUrl, "Host");
            ArgumentException.ThrowIfNullOrWhiteSpace(userName, "UserName");
            ArgumentException.ThrowIfNullOrWhiteSpace(password, "Password");

            config.SetKebabCaseEndpointNameFormatter();

            if(assembly is not null)
                config.AddConsumers(assembly);

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(hostUrl), host =>
                {
                    host.Username(userName);
                    host.Password(password);
                });
                configurator.ConfigureEndpoints(context);
            });            
        });

        return services;
    }
}
