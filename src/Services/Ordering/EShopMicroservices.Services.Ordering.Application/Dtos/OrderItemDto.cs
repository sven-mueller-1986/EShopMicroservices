namespace EShopMicroservices.Services.Ordering.Application.Dtos;

public record OrderItemDto
(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Price
);
