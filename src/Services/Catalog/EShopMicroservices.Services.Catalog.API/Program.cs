using EShopMicroservices.BuildingBlocks.Extensions;
using EShopMicroservices.BuildingBlocks.Logging;
using EShopMicroservices.BuildingBlocks.Logging.Behaviors;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog integration defined in BuildingBlocks project
builder.AddSeriLogger();

// Add services to DI.
var assembly = typeof(Program).Assembly;

// Endpoint Mapping
// Register ICarterModule implementations
builder.Services.AddCarterFromAssembly(assembly);

// Fluent Validation -> registration of Validators
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
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

// Global exception handling with IExceptionHandler implementation
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Configure Application Health Checks
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTPS request pipeline.

// Configure Application Health Checks
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Serilog after healthchecks to exclude health checks from logging
app.UseSerilogRequestLogging();

// Endpoint Mapping
app.MapCarter();

// Global exception handling with IExceptionHandler implementation
app.UseExceptionHandler(config => { });

// Global exception handling as delegate
//app.UseExceptionHandler(app =>
//{
//    app.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception == null) return;

//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;

//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});

app.Run();
