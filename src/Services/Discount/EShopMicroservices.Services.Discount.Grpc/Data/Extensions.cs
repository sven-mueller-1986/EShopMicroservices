using Microsoft.EntityFrameworkCore;

namespace EShopMicroservices.Services.Discount.Grpc.Data;

public static class Extensions
{
    // Automatically create SQLite DB if not exists
    // Automatically apply Migrations
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        context.Database.MigrateAsync();

        return app;
    }
}

