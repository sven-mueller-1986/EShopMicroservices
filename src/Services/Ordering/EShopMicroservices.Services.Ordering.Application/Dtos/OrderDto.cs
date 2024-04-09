namespace EShopMicroservices.Services.Ordering.Application.Dtos;

public record OrderDto
(
    Guid Id,
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    OrderStatus Status,
    PaymentDto Payment,
    List<OrderItemDto> OrderItems
);
