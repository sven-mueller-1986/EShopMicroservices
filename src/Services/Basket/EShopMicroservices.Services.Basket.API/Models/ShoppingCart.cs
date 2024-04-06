namespace EShopMicroservices.Services.Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

    // Mapping Constructor
    public ShoppingCart()
    { }

    public ShoppingCart(string uerName)
    {
        UserName = uerName;
    }
}

