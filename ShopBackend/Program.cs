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

builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();

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
app.MapGet("/get_product", GetProductByIdAsync);
app.MapPost("/add_product", AddProductAsync);
app.MapPost("/update_product", UpdateProductAsync);
app.MapPost("/update_product_by_id", UpdateProductByIdAsync);
app.MapPost("/delete_product", DeleteProductAsync);

async Task<IResult> GetProductsAsync(
	IProductRepository productRepository,
	CancellationToken cancellationToken,
	ILogger<Program> logger)
{
	try
	{
		logger.LogInformation("Запускаем ендпоинт get_products");
		await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
		logger.LogInformation("Пытаемся получить товары");
		var products = await productRepository.GetProducts(cancellationToken: cancellationToken);
		return Results.Ok(products);
	}
	catch (OperationCanceledException)
	{
		logger.LogInformation("Операция отменена");
		return Results.NoContent();
	}
}

async Task<IResult> GetProductByIdAsync(
	[FromQuery] Guid id,
	IProductRepository productRepository,
	CancellationToken cancellationToken
	)
{

	var product = await productRepository.GetProductById(id, cancellationToken);
	if (product == null)
	{
		return Results.NotFound($"Товар не найден");
	}
	return Results.Ok(product);

}

async Task<IResult> AddProductAsync(
	Product product,
	IProductRepository productRepository,
	HttpContext context,
	CancellationToken cancellationToken)
{
	try
	{
		await productRepository.AddProduct(product, cancellationToken);
		return Results.Created("Товар создан", product);
	}
	catch (Exception ex)
	{
		return Results.Problem(ex.ToString());
	}

}

async Task<IResult> UpdateProductAsync(
	IProductRepository productRepository,
	Product product,
	CancellationToken cancellationToken)
{
	try
	{
		await productRepository.UpdateProduct(product, cancellationToken);
		return Results.Ok();
	}
	catch (Exception ex)
	{
		return Results.NotFound($"Товар не найден");
	}

}

async Task<IResult> UpdateProductByIdAsync(
	[FromQuery] Guid id,
	[FromBody] Product updatedProduct,
	IProductRepository productRepository,
	CancellationToken cancellationToken)
{
	
	try
	{
		var product = await productRepository.GetProductById(id, cancellationToken);

		product.Name = updatedProduct.Name;
		product.Price = updatedProduct.Price;
		product.Img = updatedProduct.Img;

		await productRepository.UpdateProduct(product, cancellationToken);
		return Results.Ok();
	}
	catch (Exception ex)
	{
		return Results.NotFound($"Товар не найден");
	}

}

async Task<IResult> DeleteProductAsync(
	IProductRepository productRepository,
	[FromBody] Product product,
	CancellationToken cancellationToken)
{
	try
	{
		await productRepository.DeleteProduct(product, cancellationToken);
		return Results.Ok();
	}
	catch (Exception ex)
	{
		return Results.NotFound($"Товар не найден");
	}


}


app.Run();
