namespace EShopMicroservices.WebApps.Web.Pages;

public class OrderListModel(IOrderingService orderingService, ILogger<OrderListModel> logger)
    : PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        logger.LogInformation("Product detail page visited.");

        var response = await orderingService.GetOrdersByCustomer(new Guid("bd17ff4d-0775-47bd-91f5-4f32ddbf7c7b"));
        Orders = response.Orders;

        return Page();
    }
}
