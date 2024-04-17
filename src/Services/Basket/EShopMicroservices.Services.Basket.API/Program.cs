using Discount.Grpc;
using EShopMicroservices.BuildingBlocks.Extensions;
using EShopMicroservices.BuildingBlocks.Logging;
using EShopMicroservices.BuildingBlocks.Logging.Behaviors;
using EShopMicroservices.BuildingBlocks.Messaging.MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSeriLogger();

// Add services to DI.
var assembly = typeof(Program).Assembly;

// Services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    // Decorate IBasketRepository with Scrutor Framework
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// gRPC Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
    // Only for local development
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

// Endpoint Mapping
// Register ICarterModule implementations
builder.Services.AddCarterFromAssembly(assembly);

// Fluent Validation
// Register custom Validators
builder.Services.AddValidatorsFromAssembly(assembly);

// MediatR [C]ommand [Q]uery [R]esponsibility [S]egregation Framework
// Register Queries, Commands and Handlers
// Register Pipline Pre / Post Behaviors
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Transactional Document Database 
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Message Broker registration for async service communication
builder.Services.AddMessageBroker(builder.Configuration);

// Global exception handling with IExceptionHandler implementation
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Configure Application Health Checks
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

// Configure the HTTPS request pipeline.

// Global exception handling with IExceptionHandler implementation
app.UseExceptionHandler(config => { });

// Configure Application Health Checks
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseSerilogRequestLogging();

// Endpoint Mapping
app.MapCarter();

app.Run();
