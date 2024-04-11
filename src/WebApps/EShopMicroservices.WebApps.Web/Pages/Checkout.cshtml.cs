namespace EShopMicroservices.WebApps.Web.Pages;

public class CheckoutModel(IBasketService basketService, ILogger<CheckoutModel> logger)
    : PageModel
{
    [BindProperty]
    public BasketCheckoutModel Order { get; set; } = new();

    public ShoppingCartModel Cart { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Checkout page visited.");

        Cart = await basketService.LoadUserBasket();

        return Page();
    }

    public async Task<IActionResult> OnPostCheckOutAsync()
    {
        logger.LogInformation("Checkout cart.");

        Cart = await basketService.LoadUserBasket();

        if(!ModelState.IsValid)
            return Page();

        Order.CustomerId = new Guid("bd17ff4d-0775-47bd-91f5-4f32ddbf7c7b");
        Order.UserName = Cart.UserName;
        Order.TotalPrice = Cart.TotalPrice;

        await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}
