using Microsoft.AspNetCore.RateLimiting;
using EShopMicroservices.BuildingBlocks.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to DI.
builder.AddSeriLogger();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

// Configure the HTTPS request pipeline.

app.UseRateLimiter();

app.UseSerilogRequestLogging();

app.MapReverseProxy();

app.Run();
