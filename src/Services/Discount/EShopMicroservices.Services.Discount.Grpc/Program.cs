using EShopMicroservices.BuildingBlocks.Logging;
using EShopMicroservices.Services.Discount.Grpc.Data;
using EShopMicroservices.Services.Discount.Grpc.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSeriLogger();

builder.Services.AddGrpc();

builder.Services.AddSqlite<DiscountContext>(builder.Configuration.GetConnectionString("Database"));

builder.Services.AddHealthChecks()
                .AddSqlite(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure Application Health Checks
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Automatically initialize DB
app.UseMigration();

app.UseSerilogRequestLogging();

app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
