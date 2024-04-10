using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EShopMicroservices.BuildingBlocks.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddCarter(null, config =>
        {
            var modules = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
            config.WithModules(modules);
        });

        return services;
    }
}
