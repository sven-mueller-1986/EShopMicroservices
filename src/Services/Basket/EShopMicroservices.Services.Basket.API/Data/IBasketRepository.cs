namespace EShopMicroservices.Services.Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default);
    Task<ShoppingCart> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    Task<ShoppingCart> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default);
}

