using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopMicroservices.Services.Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(
                orderId => orderId.Value,
                dbId => OrderId.Of(dbId));

        builder.ComplexProperty(o => o.OrderName, columnBuilder =>
        {
            columnBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
        });

        builder.ComplexProperty(o => o.ShippingAddress, columnBuilder =>
        {
            columnBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

            columnBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

            columnBuilder.Property(a => a.Email)
                    .HasMaxLength(255);

            columnBuilder.Property(a => a.Line)
                    .HasMaxLength(255)
                    .IsRequired();

            columnBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

            columnBuilder.Property(a => a.State)
                    .HasMaxLength(50);

            columnBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(10)
                    .IsRequired();
        });

        builder.ComplexProperty(o => o.BillingAddress, columnBuilder =>
        {
            columnBuilder.Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

            columnBuilder.Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

            columnBuilder.Property(a => a.Email)
                    .HasMaxLength(255);

            columnBuilder.Property(a => a.Line)
                    .HasMaxLength(255)
                    .IsRequired();

            columnBuilder.Property(a => a.Country)
                    .HasMaxLength(50);

            columnBuilder.Property(a => a.State)
                    .HasMaxLength(50);

            columnBuilder.Property(a => a.ZipCode)
                    .HasMaxLength(10)
                    .IsRequired();
        });

        builder.ComplexProperty(o => o.Payment, columnBuilder =>
        {
            columnBuilder.Property(p => p.CardName)
                    .HasMaxLength(50)
                    .IsRequired();

            columnBuilder.Property(p => p.CardNumber)
                    .HasMaxLength(24)
                    .IsRequired();

            columnBuilder.Property(p => p.Expiration)
                    .HasMaxLength(10);

            columnBuilder.Property(p => p.CVV)
                    .HasMaxLength(3);

            columnBuilder.Property(p => p.PaymentMethod);
        });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);
    }
}
