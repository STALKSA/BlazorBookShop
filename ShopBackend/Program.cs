using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddHttpClient();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy =>
{
	policy
		.AllowAnyMethod()
		.AllowAnyHeader()
		.AllowAnyOrigin();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/get_products", GetProductsAsync);
app.MapGet("/get_product", GetProductAsync);
app.MapPost("/add_product", AddProductAsync);
app.MapPost("/update_product", UpdateProductAsync);
app.MapPost("/update_product_by_id", UpdateProductByIdAsync);
app.MapPost("/delete_product", DeleteProductAsync);
app.MapPost("/delete_product_by_id", DeleteProductByIdAsync);

async Task<Product[]> GetProductsAsync(AppDbContext dbContext)
{
    return await dbContext.Products.ToArrayAsync();
}

async Task<IResult> GetProductAsync([FromQuery] Guid id, AppDbContext dbContext)
{
	var foundProduct = await dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
	if (foundProduct == null)
	{
		return Results.NotFound($"Товар не найден");
	}
	return Results.Ok(foundProduct);

}

async Task AddProductAsync(Product product, AppDbContext dbContext, HttpContext context)
{
	await dbContext.Products.AddAsync(product);
	await dbContext.SaveChangesAsync();
    context.Response.StatusCode = StatusCodes.Status201Created;
}

async Task<IResult> UpdateProductAsync(AppDbContext dbContext, Product product)
{
	var foundProduct = await dbContext.Products.Where(p => p.Id == product.Id).FirstOrDefaultAsync();
	if (foundProduct == null)
	{
		return Results.NotFound($"Product with Id {product.Id} not found");
	}

	foundProduct.Name = product.Name;
	foundProduct.Price = product.Price;
	await dbContext.SaveChangesAsync();

	return Results.Ok();
}

async Task UpdateProductByIdAsync([FromQuery] Guid id, [FromBody] Product updatedProduct, AppDbContext dbContext, HttpContext context)
{
    var product = await dbContext.Products.FindAsync(id);
    if (product != null)
    {
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        await dbContext.SaveChangesAsync();
        context.Response.StatusCode = StatusCodes.Status200OK;
    }
}

async Task<IResult> DeleteProductAsync(AppDbContext dbContext, [FromBody] Product product)
{
	var deletedCount = await dbContext.Products.Where(p => p.Id == product.Id).ExecuteDeleteAsync();

	if (deletedCount == 0)
	{
		return Results.NotFound($"Продукт не найден");
	}
	await dbContext.SaveChangesAsync();
	return Results.Ok();
}
async Task<IResult> DeleteProductByIdAsync(AppDbContext dbContext, [FromQuery] Guid id)
{
	var deletedCount = await dbContext.Products.Where(p => p.Id == id).ExecuteDeleteAsync();

	if (deletedCount == 0)
	{
		return Results.NotFound($"Продукт не найден");
	}
	await dbContext.SaveChangesAsync();
	return Results.Ok();
}


app.Run();
