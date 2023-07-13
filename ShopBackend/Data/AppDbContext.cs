using Microsoft.EntityFrameworkCore;
using ShopBackend.Data;

public class AppDbContext : DbContext
{
	//Список таблиц:
	public DbSet<Product> Products => Set<Product>();

	public AppDbContext(
		DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}
}

