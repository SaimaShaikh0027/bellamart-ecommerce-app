using System.Text.Json;
using bellamart_ecommerce_app.Data;
using Microsoft.AspNetCore.Mvc;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Controllers;

public class CartController : Controller
{
    private const string CartSessionKey = "BellamartCart";
    private readonly AppDbContext _db;

    public CartController(AppDbContext db) => _db = db;

    public IActionResult Index()
    {
        var cart = GetCart();
        ViewBag.Total = cart.Sum(item => item.LineTotal);
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int productId, int quantity = 1)
    {
        var product = _db.Products.Find(productId);
        if (product is null) return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(cartItem => cartItem.Product.Id == productId);
        if (item is null)
        {
            cart.Add(new CartItem { Product = product, Quantity = Math.Clamp(quantity, 1, 99) });
        }
        else
        {
            item.Quantity = Math.Clamp(item.Quantity + quantity, 1, 99);
        }

        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Increase(int productId)
    {
        UpdateQuantity(productId, 1);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Decrease(int productId)
    {
        UpdateQuantity(productId, -1);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(item => item.Product.Id == productId);
        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (cart.Count == 0) return RedirectToAction(nameof(Index));

        ViewBag.Total = cart.Sum(item => item.LineTotal);
        return View(new CheckoutViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(CheckoutViewModel model)
    {
        var cart = GetCart();
        ViewBag.Total = cart.Sum(item => item.LineTotal);
        if (!ModelState.IsValid || cart.Count == 0) return View(model);

        var customerId = HttpContext.Session.GetInt32("BellamartUserId");
        var order = new Order
        {
            OrderNumber = (_db.Orders.Max(order => (int?)order.OrderNumber) ?? 1000) + 1,
            UserId = customerId,
            CustomerName = HttpContext.Session.GetString("BellamartUser") ?? "Guest",
            Total = cart.Sum(item => item.LineTotal),
            PlacedAt = DateTime.Now,
            PaymentMethod = model.PaymentMethod,
            Address = model.Address
        };
        order.SetItems(cart);
        _db.Orders.Add(order);
        _db.SaveChanges();
        _db.Payments.Add(new Payment
        {
            OrderId = order.Id,
            UserId = customerId,
            Amount = order.Total,
            Method = model.PaymentMethod,
            Status = model.PaymentMethod == "Cash On Delivery" ? "Pending" : "Recorded",
            RecordedAt = order.PlacedAt
        });
        _db.SaveChanges();
        HttpContext.Session.Remove(CartSessionKey);
        TempData["CartMessage"] = $"Order placed successfully with {model.PaymentMethod}.";
        return RedirectToAction(nameof(Index));
    }

    private void UpdateQuantity(int productId, int change)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(cartItem => cartItem.Product.Id == productId);
        if (item is null) return;

        item.Quantity += change;
        if (item.Quantity <= 0) cart.Remove(item);
        SaveCart(cart);
    }

    private List<CartItem> GetCart()
    {
        var json = HttpContext.Session.GetString(CartSessionKey);
        return string.IsNullOrWhiteSpace(json)
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }
}
