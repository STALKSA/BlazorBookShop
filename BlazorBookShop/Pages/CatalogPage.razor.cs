using BlazorBookShop.Models;


namespace BlazorBookShop.Pages
{
	public partial class CatalogPage
	{
		private Product[]? _products;
		private CancellationTokenSource _cts = new();

		protected override async Task OnInitializedAsync()
		{
			await GetProducts();
		}

		void NavigateToProductPage(Guid id)
		{
			maneger.NavigateTo($"/products/{id.ToString()}");
		}

		private async Task GetProducts()
		{
			_products = await ShopClient.GetProducts(_cts.Token);
		}

		public void Dispose()
		{
			_cts.Cancel();
		}

	}
}
