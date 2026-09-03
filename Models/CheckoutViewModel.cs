using System.ComponentModel.DataAnnotations;

namespace bellamart_ecommerce_app.Models;

public class CheckoutViewModel
{
    [Required, Display(Name = "Delivery address")]
    public string Address { get; set; } = "";

    [Required, Display(Name = "Payment method")]
    public string PaymentMethod { get; set; } = "Cash On Delivery";
}
