using bellamart_ecommerce_app.Data;
using Microsoft.AspNetCore.Mvc;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (_db.Users.Any(user => user.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
        }

        if (!ModelState.IsValid) return View(model);

        var user = new User { Name = model.FullName, Email = model.Email, Password = model.Password };
        _db.Users.Add(user);
        _db.SaveChanges();
        HttpContext.Session.SetString("BellamartUser", user.Name);
        HttpContext.Session.SetInt32("BellamartUserId", user.Id);
        return RedirectToAction("Index", "Product");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel { });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var account = _db.Users.SingleOrDefault(user => user.Email == model.Email);
        if (account is null || account.Password != model.Password)
        {
            ModelState.AddModelError(string.Empty, "Email or password is incorrect.");
            return View(model);
        }

        HttpContext.Session.SetString("BellamartUser", account.Name);
        HttpContext.Session.SetInt32("BellamartUserId", account.Id);
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", "Product");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("BellamartUser");
        HttpContext.Session.Remove("BellamartUserId");
        return RedirectToAction("Index", "Product");
    }
}
