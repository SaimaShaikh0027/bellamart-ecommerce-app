using bellamart_ecommerce_app.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Controllers;

public class OrderController : Controller
{
    private readonly AppDbContext _db;

    public OrderController(AppDbContext db) => _db = db;

    public IActionResult Index()
    {
        var customerId = HttpContext.Session.GetInt32("BellamartUserId");
        var orders = customerId is null
            ? new List<Order>()
            : _db.Orders.AsNoTracking().Where(order => order.UserId == customerId).ToList();

        foreach (var order in orders) order.LoadItems();
        return View(orders);
    }
}
