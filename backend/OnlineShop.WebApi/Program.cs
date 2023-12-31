
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data.EntityFramework.Repositories;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.IdentityPasswordHasherLib;
using OnlineShop.WebApi.Configurations;
using OnlineShop.WebApi.Data.Repositories;
using OnlineShop.WebApi.Filters;
using OnlineShop.WebApi.Middleware;
using OnlineShop.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

JwtConfig jwtConfig = builder.Configuration
    .GetRequiredSection("JwtConfig")
    .Get<JwtConfig>()!;
if (jwtConfig is null) throw new InvalidOperationException("JwtConfig не сконфигурирован!");
builder.Services.AddSingleton(jwtConfig);

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(order: 1);
    options.Filters.Add<AuthentificationFilter>(order: 0);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddHttpClient();




//builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEf>();
builder.Services.AddScoped<ICartRepository, CartRepositoryEf>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddSingleton<IApplicationPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddSingleton<ITransitionCounterService, TransitionCounterService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IConfirmationCodeRepository, ConfirmationCodeRepositoryEf>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEf>();

//builder.Services.AddMediatR(cfg =>
//{
//    cfg.RegisterServicesFromAssemblies(typeof(AccountRegistered).Assembly);
//    cfg.RegisterServicesFromAssemblies(typeof(LoginCodeCreated).Assembly);
//});

builder.Services.AddHttpLogging(options => 
{
    options.LoggingFields = HttpLoggingFields.RequestHeaders
                            | HttpLoggingFields.ResponseHeaders;
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,

            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudiences = new[] { jwtConfig.Audience },
            ValidIssuer = jwtConfig.Issuer
        };
    });
builder.Services.AddAuthorization();



var app = builder.Build();

app.UseMiddleware<TransitionsCounterMiddleware>();

app.Use(async (context, next) =>
{
    var userAgent = context.Request.Headers.UserAgent;
    if (userAgent.ToString().Contains("Edg"))
    {
        await next();
    }
    else
    {
        await context.Response.WriteAsync("Сайт поддерживается только на браузере Edge");
    }

});


// app.UseHttpLogging();

app.UseCors(policy =>
{
	policy
        .WithOrigins("https://localhost:5001")
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/get_products", GetProductsAsync);
//app.MapGet("/get_product", GetProductByIdAsync);
//app.MapPost("/add_product", AddProductAsync);
//app.MapPost("/update_product", UpdateProductAsync);
//app.MapPost("/update_product_by_id", UpdateProductByIdAsync);
//app.MapPost("/delete_product", DeleteProductAsync);

//app.MapGet("/", (IAccountRepository repo) => { });

//async Task<IResult> GetProductsAsync(
//	IRepository<Product> repository,
//	CancellationToken cancellationToken
//	)
//{
//	try
//	{
	
//		var products = await repository.GetAll(cancellationToken);
//		return Results.Ok(products);
//	}
//	catch (Exception ex)
//	{
//		return Results.Empty;
//	}
//}

//async Task<IResult> GetProductByIdAsync(
//	[FromQuery] Guid id,
//	IRepository<Product> repository,
//	CancellationToken cancellationToken
//	)
//{
//	try
//	{

//		var product = await repository.GetById(id, cancellationToken);
//		return Results.Ok(product);
//	}
//	catch (InvalidOperationException ex)
//	{
//		return Results.NotFound();
//	}
	

//}

//async Task<IResult> AddProductAsync(
//	Product product,
//	IRepository<Product> repository,
//	HttpContext context,
//	CancellationToken cancellationToken)
//{
//	try
//	{
//		await repository.Add(product, cancellationToken);
//		return Results.Created("Товар создан", product);
//	}
//	catch (Exception ex)
//	{
//		return Results.Problem(ex.ToString());
//	}

//}

//async Task<IResult> UpdateProductAsync(
//	IRepository<Product> repository,
//	Product product,
//	CancellationToken cancellationToken)
//{
//	try
//	{
//		await repository.Update(product, cancellationToken);
//		return Results.Ok();
//	}
//	catch (Exception ex)
//	{
//		return Results.NotFound($"Товар не найден");
//	}

//}

//async Task<IResult> UpdateProductByIdAsync(
//	[FromQuery] Guid id,
//	[FromBody] Product updatedProduct,
//	IRepository<Product> repository,
//	CancellationToken cancellationToken)
//{
	
//	try
//	{
//		var product = await repository.GetById(id, cancellationToken);

//		product.Name = updatedProduct.Name;
//		product.Price = updatedProduct.Price;
//		product.Img = updatedProduct.Img;

//		await repository.Update(product, cancellationToken);
//		return Results.Ok();
//	}
//	catch (Exception ex)
//	{
//		return Results.NotFound($"Товар не найден");
//	}

//}

//async Task<IResult> DeleteProductAsync(
//	IRepository<Product> repository,
//	[FromBody] Product product,
//	CancellationToken cancellationToken)
//{
//	try
//	{
//		await repository.Delete(product, cancellationToken);
//		return Results.Ok();
//	}
//	catch (Exception ex)
//	{
//		return Results.NotFound($"Товар не найден");
//	}


//}


app.Run();
