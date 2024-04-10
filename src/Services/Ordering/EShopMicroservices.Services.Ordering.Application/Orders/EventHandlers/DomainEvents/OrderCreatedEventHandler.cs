using MassTransit;
using Microsoft.FeatureManagement;

namespace EShopMicroservices.Services.Ordering.Application.Orders.EventHandlers.DomainEvents;

public class OrderCreatedEventHandler(IPublishEndpoint messageService, ILogger<OrderCreatedEventHandler> logger, IFeatureManager featureManager)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        if (!await featureManager.IsEnabledAsync("OrderFullfilment"))
            return;

        var orderCreatedIntegrationEvent = domainEvent.order.ToDto();

        await messageService.Publish(orderCreatedIntegrationEvent, cancellationToken);
    }
}