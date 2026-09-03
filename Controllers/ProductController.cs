using bellamart_ecommerce_app.Data;
using Microsoft.AspNetCore.Mvc;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _db;

    public ProductController(AppDbContext db) => _db = db;

    public IActionResult Index()
    {
        return View(_db.Products.OrderBy(product => product.Id).ToList());
    }

    public IActionResult Details(int id)
    {
        var product = _db.Products.Find(id);
        return product is null ? NotFound() : View(product);
    }

    private static readonly List<Product> Products = new()
    {
            
            new() { Id = 1, Name = "Velvet Matte Lipstick", Price = 499, Category = "Beauty", ImageUrl = "/img/products/VelvetMatteLipstick.webp", Description = "Long lasting matte finish lipstick with rich, comfortable color for all-day wear." },
            new() { Id = 2, Name = "Daily Glow Face Wash", Price = 299, Category = "Beauty", ImageUrl = "/img/products/DailyGlowFacewash.webp", Description = "A gentle daily cleanser that leaves skin feeling fresh, soft, and beautifully balanced." },
            new() { Id = 3, Name = "Linen Blend Overshirt", Price = 1299, Category = "Fashion", ImageUrl = "/img/products/LinenBlendOvershirt.jpg", Description = "A relaxed, breathable layer made for easy weekends and warm-weather days." },
            new() { Id = 4, Name = "Everyday Canvas Sneakers", Price = 1899, Category = "Fashion", ImageUrl = "/img/products/EveryDayCanvasSneaker.webp", Description = "Lightweight canvas sneakers with a timeless shape and cushioned everyday comfort." },
            new() { Id = 5, Name = "Ceramic Pour-Over Set", Price = 899, Category = "Home", ImageUrl = "/img/products/CeremicPour-OverSetform.webp", Description = "A calm morning ritual in ceramic, including a dripper and matching coffee cup." },
            new() { Id = 6, Name = "Cloud Soft Throw", Price = 1499, Category = "Home", ImageUrl = "/img/products/CloudSoftThrow.webp", Description = "A soft, textured throw that adds warmth and a quiet layer of comfort to any room." },
            new() { Id = 7, Name = "Wireless Noise-Cancel Headphones", Price = 3499, Category = "Tech", ImageUrl = "/img/products/WirelessNoice-CancelAndHeadphones.webp", Description = "Immersive wireless sound with active noise cancellation for focused listening anywhere." },
            new() { Id = 8, Name = "Smart Desk Lamp", Price = 2199, Category = "Tech", ImageUrl = "/img/products/SmartDeskLamp.webp", Description = "A modern adjustable lamp with warm, focused light for work and winding down." },
            new() { Id = 9, Name = "Hydrating Hand Cream", Price = 649, Category = "Beauty", ImageUrl = "/img/products/HydratingHandCream.webp", Description = "A fast-absorbing hand cream that gives dry hands lasting, lightweight hydration." },
            new() { Id = 10, Name = "Ribbed Lounge Set", Price = 1599, Category = "Fashion", ImageUrl = "/img/products/RibbedLounge.webp", Description = "A soft ribbed co-ord designed for slow mornings, travel days, and relaxed evenings." },
            new() { Id = 11, Name = "Sculptural Table Vase", Price = 799, Category = "Home", ImageUrl = "/img/products/SculpturalVase.webp", Description = "A sculptural ceramic accent that brings an artful touch to shelves and tables." },
            new() { Id = 12, Name = "Portable Mini Speaker", Price = 2499, Category = "Tech", ImageUrl = "/img/products/MiniSpeaker.webp", Description = "Compact, room-filling audio with a portable design made for every gathering." }
    };

    public static List<Product> GetProducts() => Products;
}