using EShopMicroservices.BuildingBlocks.Pagination;

namespace EShopMicroservices.WebApps.Web.Models.Ordering;

public record CreateOrderResponse(Guid Id);
public record DeleteOrderResponse(bool IsSuccess);
public record GetOrdersResponse(PaginatedResult<OrderModel> Orders);
public record GetOrdersByCustomerResponse(IEnumerable<OrderModel> Orders);
public record GetOrdersByNameResponse(IEnumerable<OrderModel> Orders);
public record UpdateOrderResponse(bool IsSuccess);
