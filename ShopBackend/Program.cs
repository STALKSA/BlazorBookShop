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
app.MapGet("/get_product", GetProductAsync);
app.MapPost("/add_product", AddProductAsync);
app.MapPost("/update_product", UpdateProductAsync);
//app.MapPost("/update_product_by_id", UpdateProductByIdAsync);
app.MapPost("/delete_product", DeleteProductAsync);
//app.MapPost("/delete_product_by_id", DeleteProductByIdAsync);

async Task<IResult> GetProductsAsync(
	IProductRepository productRepository,
	CancellationToken cancellationToken,
	ILogger<Program> logger)
{
	try
	{
		logger.LogInformation("Çàïóñòèëè åíäïîèíò get_products");
		await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
		logger.LogInformation("Ïûòàåìñÿ ïîëó÷èòü òîâàðû");
		var products = await productRepository.GetProducts(cancellationToken: cancellationToken);
		return Results.Ok(products);
	} catch (OperationCanceledException)
	{
		logger.LogInformation("Îïåðàöèÿ îòìåíåíà");
		return Results.NoContent();
	}
}

async Task<IResult> GetProductAsync(
	[FromQuery] Guid id, 
	IProductRepository productRepository, 
	CancellationToken cancellationToken
	)
{
	
	var product = await productRepository.GetProductById(id, cancellationToken);
	if (product == null)
	{
		return Results.NotFound($"Òîâàð íå íàéäåí");
	}
	return Results.Ok(product);

}

async Task<IResult> AddProductAsync(
	Product product, 
	IProductRepository productRepository, 
	HttpContext context,
	CancellationToken cancellationToken)
{
	await productRepository.Add(product, cancellationToken);
	return Results.Ok(product);
	
}

async Task<IResult> UpdateProductAsync(
	IProductRepository productRepository,
	Product product,
	CancellationToken cancellationToken)
{

	if (product == null)
	{
		return Results.NotFound($"Ïðîäóêò {product.Id} íå íàéäåí");
	}
	await productRepository.Update(product, cancellationToken);
	return Results.Ok();

}

//async Task<IResult> UpdateProductByIdAsync(
//	[FromQuery] Guid id, 
//	[FromBody] Product updatedProduct,
//	IProductRepository productRepository,
//	CancellationToken cancellationToken)
//{
//    var updatedProduct = await productRepository.Update(id, cancellationToken);
//	return Results.Ok();
//    if (product != null)
//    {
//        product.Name = updatedProduct.Name;
//        product.Price = updatedProduct.Price;
//        await dbContext.SaveChangesAsync();
//        context.Response.StatusCode = StatusCodes.Status200OK;
//    }
//}

async Task<IResult> DeleteProductAsync(
	IProductRepository productRepository, 
	[FromBody] Product product,
	CancellationToken cancellationToken)
{
	 await productRepository.Delete(product,cancellationToken);
	return Results.Ok();	
}
//async Task<IResult> DeleteProductByIdAsync(AppDbContext dbContext, [FromQuery] Guid id)
//{
//	var deletedCount = await dbContext.Products.Where(p => p.Id == id).ExecuteDeleteAsync();

//	if (deletedCount == 0)
//	{
//		return Results.NotFound($"Ïðîäóêò íå íàéäåí");
//	}
//	await dbContext.SaveChangesAsync();
//	return Results.Ok();
//} 


app.Run();
