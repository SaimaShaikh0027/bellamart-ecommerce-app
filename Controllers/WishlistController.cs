using System.Text.Json;
using bellamart_ecommerce_app.Data;
using Microsoft.AspNetCore.Mvc;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Controllers;

public class WishlistController : Controller
{
    private const string WishlistSessionKey = "BellamartWishlist";
    private readonly AppDbContext _db;

    public WishlistController(AppDbContext db) => _db = db;

    public IActionResult Index() => View(GetWishlist());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int productId)
    {
        var product = _db.Products.Find(productId);
        if (product is null) return NotFound();

        var wishlist = GetWishlist();
        if (wishlist.All(item => item.Id != productId)) wishlist.Add(product);
        SaveWishlist(wishlist);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        var wishlist = GetWishlist();
        wishlist.RemoveAll(item => item.Id == productId);
        SaveWishlist(wishlist);
        return RedirectToAction(nameof(Index));
    }

    private List<Product> GetWishlist()
    {
        var json = HttpContext.Session.GetString(WishlistSessionKey);
        return string.IsNullOrWhiteSpace(json)
            ? new List<Product>()
            : JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
    }

    private void SaveWishlist(List<Product> wishlist)
    {
        HttpContext.Session.SetString(WishlistSessionKey, JsonSerializer.Serialize(wishlist));
    }
}
