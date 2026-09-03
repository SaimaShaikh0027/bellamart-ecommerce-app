using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace bellamart_ecommerce_app.Models;

public class Order
{
    public int Id { get; set; }

    public int OrderNumber { get; set; }

    public int? UserId { get; set; }

    public string CustomerName { get; set; } = "";

    public decimal Total { get; set; }

    public DateTime PlacedAt { get; set; }

    public string PaymentMethod { get; set; } = "";

    public string Address { get; set; } = "";

    public string ItemsJson { get; set; } = "[]";

    [NotMapped]
    public List<CartItem> Items { get; set; } = new();

    public void SetItems(List<CartItem> items)
    {
        Items = items;
        ItemsJson = JsonSerializer.Serialize(items);
    }

    public void LoadItems()
    {
        Items = JsonSerializer.Deserialize<List<CartItem>>(ItemsJson) ?? new List<CartItem>();
    }
}
