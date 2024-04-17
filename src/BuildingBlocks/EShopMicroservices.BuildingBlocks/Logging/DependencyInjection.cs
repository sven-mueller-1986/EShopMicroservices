using EShopMicroservices.BuildingBlocks.Logging.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace EShopMicroservices.BuildingBlocks.Logging;

public static class DependencyInjection
{
    public static void AddSeriLogger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog(config =>
        {
            var elasticsearchUri = builder.Configuration["ElasticConfiguration:Uri"];
            ArgumentException.ThrowIfNullOrWhiteSpace(elasticsearchUri, "ElasticConfiguration:Uri");

            var env = builder.Environment.EnvironmentName;
            var appName = builder.Environment.ApplicationName;

            config.Enrich.FromLogContext()
                  .Enrich.WithMachineName()
                  .Enrich.WithProperty("Environment", env)
                  .Enrich.WithProperty("Application", appName)
                  .WriteTo.Console()
                  .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUri))
                  {
                      IndexFormat = $"applogs-{appName.ToLower().Replace(".", "-")}-{env.ToLower().Replace(".", "-")}-logs-{DateTime.UtcNow:yyyy-MM}",
                      AutoRegisterTemplate = true,
                      NumberOfShards = 2,
                      NumberOfReplicas = 1,
                  })
                  .ReadFrom.Configuration(builder.Configuration);
        });

        builder.Services.AddTransient<HttpLoggingHandler>();
    }
}
