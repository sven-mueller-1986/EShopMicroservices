using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;

namespace EShopMicroservices.Services.Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        var policy = Policy
            .Handle<SqlException>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(1000) * retryAttempt,
                onRetry: (exception, retryTime, context) =>
                {
                    Log.Error("Retry {RetryTime} of {Policy} at {Operation}, due to {Exception}.", retryTime, context.PolicyKey, context.OperationKey, exception);
                });

        await policy.ExecuteAsync(async () =>
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            await SeedAsync(context);
        });        
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomersAsync(context);
        await SeedProductsAsync(context);
        await SeedOrdersWithItemsAsync(context);
    }

    private static async Task SeedCustomersAsync(ApplicationDbContext context)
    {
        if (await context.Customers.AnyAsync())
            return;

        await context.Customers.AddRangeAsync(InitialData.Customers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        await context.Products.AddRangeAsync(InitialData.Products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        if (await context.Orders.AnyAsync())
            return;

        await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
        await context.SaveChangesAsync();
    }
}
