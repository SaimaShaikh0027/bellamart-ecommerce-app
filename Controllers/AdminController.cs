using bellamart_ecommerce_app.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Index() => View(_db.Products.ToList());

    [HttpGet]
    public IActionResult Create() => View("Edit", new Product());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid) return View("Edit", product);

        _db.Products.Add(product);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = FindProduct(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        var existing = FindProduct(product.Id);
        if (existing is null) return NotFound();
        _db.Entry(existing).CurrentValues.SetValues(product);
        existing.Stock = Math.Max(0, product.Stock);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var product = FindProduct(id);
        if (product is not null)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Orders()
    {
        var orders = _db.Orders.AsNoTracking().ToList();
        foreach (var order in orders) order.LoadItems();
        return View(orders);
    }

    private Product? FindProduct(int id) => _db.Products.Find(id);
}
