namespace EShopMicroservices.WebApps.Web.Models.Basket;

public record CheckoutBasketRequest(BasketCheckoutModel BasketCheckoutDto);
public record StoreBasketRequest(ShoppingCartModel Cart);

