namespace EShopMicroservices.WebApps.Web.Models.Basket;

public record CheckoutBasketResponse(bool IsSuccess);
public record DeleteBasketResponse(ShoppingCartModel Cart);
public record GetBasketResponse(ShoppingCartModel Cart);
public record StoreBasketResponse(string UserName);

