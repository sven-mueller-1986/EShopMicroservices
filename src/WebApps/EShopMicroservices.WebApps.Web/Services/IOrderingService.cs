namespace EShopMicroservices.WebApps.Web.Services;

public interface IOrderingService
{
    [Post("/ordering-service/orders")]
    Task<CreateOrderResponse> CreateOrder([Body(BodySerializationMethod.Serialized)] CreateOrderRequest request);

    [Get("/ordering-service/orders?pageIndex={pageIndex}&pageSize={pageSize}")]
    Task<GetOrdersResponse> GetOrders(int pageIndex = 0, int pageSize = 10);

    [Get("/ordering-service/orders/{name}")]
    Task<GetOrdersByNameResponse> GetOrdersByName(string name);

    [Get("/ordering-service/orders/customer/{id}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid id);

    [Put("/ordering-service/orders")]
    Task<UpdateOrderResponse> UpdateOrder([Body(BodySerializationMethod.Serialized)] UpdateOrderRequest request);

    [Delete("/ordering-service/orders/{id}")]
    Task<DeleteOrderResponse> DeleteOrder(Guid id);
}
