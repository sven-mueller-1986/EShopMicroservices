namespace EShopMicroservices.WebApps.Web.Models.Ordering;

public record OrderItemModel
(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Price
);
