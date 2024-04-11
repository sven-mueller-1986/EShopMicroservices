namespace EShopMicroservices.WebApps.Web.Pages;

public class ProductDetailModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductDetailModel> logger)
    : PageModel
{
    public ProductModel Product { get; set; } = default!;

    [BindProperty]
    public string Color { get; set; } = default!;

    [BindProperty]
    public int Quantity { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid productId)
    {
        logger.LogInformation("Product detail page visited.");

        var response = await catalogService.GetProductById(productId);
        Product = response.Product;

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Product added to cart.");

        var productResponse = await catalogService.GetProductById(productId);

        var basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            Id = productId,
            Name = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = Quantity,
            Color = Color
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
