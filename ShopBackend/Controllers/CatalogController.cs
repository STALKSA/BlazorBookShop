using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
	public class CatalogController : Controller
	{
		private readonly IRepository<Product> _repository;

		public CatalogController(IRepository<Product> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		[HttpGet("get_products")]
		public async Task<ActionResult<IReadOnlyList<Product>>> GetProductsAsync(CancellationToken cancellationToken)
		{
			try
			{
				var products = await _repository.GetAll(cancellationToken);
				return Ok(products);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest();
			}
		}

		[HttpGet("get_product")]
		public async Task<ActionResult<Product>> GetProductByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken)
		{
			try
			{
				var foundProduct = await _repository.GetById(id, cancellationToken);
				return Ok(foundProduct);
			}
			catch (InvalidOperationException eх)
			{
				return NotFound();
			}
		}

		[HttpPost("add_product")]
		public async Task<IActionResult> AddProductAsync([FromBody] Product product, CancellationToken cancellationToken)
		{
			try
			{
				await _repository.Add(product, cancellationToken);
				return Created("Created!", product);
			}
			catch (InvalidOperationException ex)
			{
				return Problem(ex.ToString());
			}
		}

		[HttpPost("update_product")]
		public async Task<ActionResult> UpdateProductAsync([FromBody] Product product, CancellationToken cancellationToken)
		{
			try
			{
				await _repository.Update(product, cancellationToken);
				return Ok();
			}
			catch (InvalidOperationException ex)
			{
				return NotFound();
			}
		}

		[HttpPost("update_product_by_id")]
		public async Task<ActionResult> UpdateProductByIdAsync([FromBody] Product product, CancellationToken cancellationToken)
		{
			try
			{
				await _repository.Update(product, cancellationToken);
				return Ok();
			}
			catch (InvalidOperationException ex)
			{
				return NotFound();
			}
		}

		[HttpPost("delete_product")]
		public async Task<ActionResult> DeleteProductAsync([FromBody] Product product, CancellationToken cancellationToken)
		{
			try
			{
				await _repository.Delete(product, cancellationToken);
				return Ok();
			}
			catch (InvalidOperationException ex)
			{
				return NotFound();
			}
		}

	}
}
