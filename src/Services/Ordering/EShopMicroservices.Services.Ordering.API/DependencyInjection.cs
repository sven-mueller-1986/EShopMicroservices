﻿using EShopMicroservices.BuildingBlocks.Exceptions.Handlers;
using EShopMicroservices.BuildingBlocks.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace EShopMicroservices.Services.Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarterFromAssembly(typeof(Program).Assembly);

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(config => { });

        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app;
    }
}

