namespace EShopMicroservices.WebApps.Web.Models.Basket;

public class ShoppingCartModel
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItemModel> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}
