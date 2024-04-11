namespace EShopMicroservices.WebApps.Web.Models.Basket;

public class ShoppingCartItemModel
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
