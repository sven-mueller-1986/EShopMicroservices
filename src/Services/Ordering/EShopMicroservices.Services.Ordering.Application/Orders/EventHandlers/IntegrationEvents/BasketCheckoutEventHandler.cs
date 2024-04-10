using EShopMicroservices.BuildingBlocks.Messaging.Events;
using EShopMicroservices.Services.Ordering.Application.Orders.Commands.CreateOrder;
using MassTransit;

namespace EShopMicroservices.Services.Ordering.Application.Orders.EventHandlers.IntegrationEvents;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.EventType);       

        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var orderId = Guid.NewGuid();

        var order = new OrderDto
        (
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: new AddressDto
            (
                    message.FirstName,
                    message.LastName,
                    message.Email,
                    message.Line,
                    message.Country,
                    message.State,
                    message.ZipCode
            ),
            BillingAddress: new AddressDto
            (
                    message.FirstName,
                    message.LastName,
                    message.Email,
                    message.Line,
                    message.Country,
                    message.State,
                    message.ZipCode
            ),
            Payment: new PaymentDto
            (
                message.CardName,
                message.CardNumber,
                message.Expiration,
                message.CVV,
                message.PaymentMethod
            ),
            Status: OrderStatus.Pending,
            OrderItems:
            [
                new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 2, 800),
                new OrderItemDto(orderId, new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"), 1, 470),
            ]
        );

        return new CreateOrderCommand(order);
    }
}
