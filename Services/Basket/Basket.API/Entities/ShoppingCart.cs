namespace Basket.API.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; }

    public ShoppingCart()
    {
        Items = new List<ShoppingCartItem>();
    }

    public double TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}
