using System.ComponentModel.DataAnnotations;

namespace bellamart_ecommerce_app.Models;

public class RegisterViewModel
{
    [Required, Display(Name = "Full name")]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, DataType(DataType.Password), MinLength(6)]
    public string Password { get; set; } = "";

    [Required, Compare(nameof(Password)), DataType(DataType.Password), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = "";
}
