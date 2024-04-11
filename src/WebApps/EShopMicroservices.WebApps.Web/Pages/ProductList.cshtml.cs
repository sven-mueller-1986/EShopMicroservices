namespace EShopMicroservices.WebApps.Web.Pages;

public class ProductListModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductListModel> logger)
    : PageModel
{
    public IEnumerable<string> CategoryList { get; set; } = [];
    public IEnumerable<ProductModel> ProductList { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string SelectedCategory { get; set; } = default!;


    public async Task<IActionResult> OnGetAsync(string categoryName)
    {
        logger.LogInformation("Product page visited");

        var response = await catalogService.GetProducts();

        CategoryList = response.Products.SelectMany(x => x.Categories).Distinct();

        ProductList = response.Products;
        SelectedCategory = categoryName;

        if (!string.IsNullOrEmpty(categoryName))
        {
            ProductList = ProductList.Where(x => x.Categories.Contains(categoryName));            
        }

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
            Quantity = 1,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
