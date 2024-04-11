namespace EShopMicroservices.WebApps.Web.Models.Ordering;

public record OrderModel
(
    Guid Id,
    Guid CustomerId,
    string OrderName,
    AddressModel ShippingAddress,
    AddressModel BillingAddress,
    OrderStatus Status,
    PaymentModel Payment,
    List<OrderItemModel> OrderItems
);
