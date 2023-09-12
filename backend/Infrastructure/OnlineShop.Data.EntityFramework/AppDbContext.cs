using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;

public class AppDbContext : DbContext
{
	//Список таблиц:
	public DbSet<Product> Products => Set<Product>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartsItems => Set<CartItem>();

    public AppDbContext(
		DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}

  
}

