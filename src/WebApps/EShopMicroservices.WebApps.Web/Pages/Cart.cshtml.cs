namespace EShopMicroservices.WebApps.Web.Pages;

public class CartModel(IBasketService basketService, ILogger<CartModel> logger)
    : PageModel
{
    public ShoppingCartModel Cart { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Cart page visited");
        Cart = await basketService.LoadUserBasket();            

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveToCartAsync(Guid productId)
    {
        logger.LogInformation("Removed item from cart");
        Cart = await basketService.LoadUserBasket();

        Cart.Items.RemoveAll(x => x.Id == productId);

        await basketService.StoreBasket(new StoreBasketRequest(Cart));

        return RedirectToPage();
    }
}