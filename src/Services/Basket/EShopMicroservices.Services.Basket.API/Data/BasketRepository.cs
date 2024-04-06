namespace EShopMicroservices.Services.Basket.API.Data;

public class BasketRepository(IDocumentSession session)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);

        if (basket is null)
            throw new BasketNotFoundException(userName);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        session.Store(shoppingCart);
        await session.SaveChangesAsync();

        return shoppingCart;
    }

    public async Task<ShoppingCart> DeleteBasketAsync(string userName, CancellationToken cancellationToken)
    {
        var shoppingCart = await GetBasketAsync(userName, cancellationToken);

        session.Delete(shoppingCart);
        await session.SaveChangesAsync();

        return shoppingCart;
    }
}

