using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopMicroservices.Services.Ordering.Infrastructure.Data.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        builder.Property(oi => oi.Id).HasConversion(
                orderItemId => orderItemId.Value,
                dbId => OrderItemId.Of(dbId));

        builder.Property(oi => oi.Price).IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);
    }
}
