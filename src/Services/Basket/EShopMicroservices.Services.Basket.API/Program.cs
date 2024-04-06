var builder = WebApplication.CreateBuilder(args);

// Add services to DI.
var assembly = typeof(Program).Assembly;

// Services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
// Decorate IBasketRepository with Scrutor Framework
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Endpoint Mapping
// Register ICarterModule implementations
builder.Services.AddCarter(null, config =>
{
    var modules = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
    config.WithModules(modules);
});

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

// Global exception handling with IExceptionHandler implementation
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Configure Application Health Checks
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

// Configure the HTTPS request pipeline.

// Endpoint Mapping
app.MapCarter();

// Global exception handling with IExceptionHandler implementation
app.UseExceptionHandler(config => { });

// Configure Application Health Checks
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
