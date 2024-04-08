namespace EShopMicroservices.Services.Ordering.Infrastructure.Data.Extensions;

internal static class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("bd17ff4d-0775-47bd-91f5-4f32ddbf7c7b")), "Heiko", "Heiko@testmail.com"),
            Customer.Create(CustomerId.Of(new Guid("ad6990b2-c4dc-40ab-a651-0543ee8efb6c")), "Stephan", "Stephan@testmail.com"),
        };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "IPhone X", 950.00M),
            Product.Create(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "Samsung 10", 840.00M),
            Product.Create(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), "Huawei Plus", 650.00M),
            Product.Create(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), "Xiaomi Mi 9", 470.00M)
        };

    public static IEnumerable<Order> OrdersWithItems
    {
        get 
        {
            var address1 = Address.Of("Heiko", "Schneider", "Heiko@testmail.com", "Teststraße 12 Teststadt", "Deutschland", "Sachsen", "40792");
            var address2 = Address.Of("Stephan", "Müller", "Stephan@testmail.com", "Musterstraße 45A Musterstadt", "Deutschland", "Thüringen", "56479");

            var payment1 = Payment.Of("Heiko Schneider", "111555555559999", "12/28", "355", 1);
            var payment2 = Payment.Of("Stephan Müller", "777333333334444", "06/30", "158", 2);

            var order1 = Order.Create(
                    OrderId.Of(new Guid("cb050209-b888-4203-a9fb-98f00a4a4f04")),
                    CustomerId.Of(new Guid("bd17ff4d-0775-47bd-91f5-4f32ddbf7c7b")),
                    OrderName.Of("ORD_1"),
                    shippingAddress: address1,
                    billingAddress: address1,
                    payment1);
            order1.AddProduct(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), 3, 950.00M);
            order1.AddProduct(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), 1, 650.00M);

            var order2 = Order.Create(
                    OrderId.Of(new Guid("fa3a8949-3c4e-4a5c-b13a-c05c79e0ac0f")),
                    CustomerId.Of(new Guid("ad6990b2-c4dc-40ab-a651-0543ee8efb6c")),
                    OrderName.Of("ORD_2"),
                    shippingAddress: address2,
                    billingAddress: address2,
                    payment2);
            order2.AddProduct(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), 1, 800.00M);
            order2.AddProduct(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), 2, 470.00M);

            return new List<Order> { order1, order2 };
        }
    }
}
