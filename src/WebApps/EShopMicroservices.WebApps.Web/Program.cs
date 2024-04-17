using Serilog;
using EShopMicroservices.BuildingBlocks.Logging;
using EShopMicroservices.BuildingBlocks.Logging.Handler;
using EShopMicroservices.WebApps.Web.Policies;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSeriLogger();

builder.Services.AddRazorPages();

var apiBaseAddress = builder.Configuration["ApiSettings:GatewayAddress"];
ArgumentException.ThrowIfNullOrWhiteSpace(apiBaseAddress, "ApiSettings:GatewayAddress");

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<HttpLoggingHandler>()
    .AddPolicyHandler(RequestPolicies.RetryPolicy)
    .AddPolicyHandler(RequestPolicies.CircuitBreakerPolicy);

builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<HttpLoggingHandler>()
    .AddPolicyHandler(RequestPolicies.RetryPolicy)
    .AddPolicyHandler(RequestPolicies.CircuitBreakerPolicy);

builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<HttpLoggingHandler>()
    .AddPolicyHandler(RequestPolicies.RetryPolicy)
    .AddPolicyHandler(RequestPolicies.CircuitBreakerPolicy);

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri($"{apiBaseAddress}/health"), "API Gateway");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseRouting();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapRazorPages();

app.Run();
