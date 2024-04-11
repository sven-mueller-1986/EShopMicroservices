namespace EShopMicroservices.WebApps.Web.Services;

public interface IBasketService
{
    [Post("/basket-service/basket")]
    Task<StoreBasketResponse> StoreBasket([Body(BodySerializationMethod.Serialized)] StoreBasketRequest request);

    [Get("/basket-service/basket/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    [Delete("/basket-service/basket/{id}")]
    Task<DeleteBasketResponse> DeleteBasket(Guid id);

    [Post("/basket-service/basket/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket([Body(BodySerializationMethod.Serialized)]  CheckoutBasketRequest request);

    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        var userName = "Heiko";
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (Exception)
        {
            basket = new ShoppingCartModel
            {
                UserName = userName,
                Items = new List<ShoppingCartItemModel>()
            };
        }

        return basket;
    }
}