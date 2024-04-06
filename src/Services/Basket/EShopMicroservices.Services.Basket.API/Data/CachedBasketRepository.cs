using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EShopMicroservices.Services.Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await repository.GetBasketAsync(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        var basket = await repository.StoreBasketAsync(shoppingCart, cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await repository.DeleteBasketAsync(userName, cancellationToken);

        await cache.RemoveAsync(basket.UserName, cancellationToken);

        return basket;
    }
}

