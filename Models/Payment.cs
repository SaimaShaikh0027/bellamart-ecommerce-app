namespace bellamart_ecommerce_app.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public decimal Amount { get; set; }

    public string Method { get; set; } = "";

    public string Status { get; set; } = "";

    public DateTime RecordedAt { get; set; }
}