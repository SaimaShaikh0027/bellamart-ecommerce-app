namespace bellamart_ecommerce_app.Models;

public class CartItem
{
    public Product Product { get; set; } = new();

    public int Quantity { get; set; }

    public decimal LineTotal => Product.Price * Quantity;
}