﻿namespace EShopMicroservices.Services.Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(o => o.ToDto());
    }

    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        (
            Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto
            (
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.Email,
                order.ShippingAddress.Line,
                order.ShippingAddress.Country,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode
            ),
            BillingAddress: new AddressDto
            (
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.Email,
                order.BillingAddress.Line,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode
            ),
            Payment: new PaymentDto
            (
                order.Payment.CardName,
                order.Payment.CardNumber,
                order.Payment.Expiration,
                order.Payment.CVV,
                order.Payment.PaymentMethod
            ),
            Status: order.Status,
            OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
        );
    }
}
