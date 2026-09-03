using Microsoft.EntityFrameworkCore;
using bellamart_ecommerce_app.Models;

namespace bellamart_ecommerce_app.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<Payment> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<Payment>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
