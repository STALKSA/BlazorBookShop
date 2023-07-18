using BlazorBookShop.Models;


namespace BlazorBookShop.Pages
{
	public partial class CatalogPage : IDisposable
	{
		private Product[]? _products;
		private CancellationTokenSource _cts = new();

		protected override async Task OnInitializedAsync()
		{
			_products = await ShopClient.GetProducts(_cts.Token);
			
		}
		void NavigateToProductPage(Guid id)
		{
			maneger.NavigateTo($"/products/{id.ToString()}");
		}

		public void Dispose()
		{
			_cts.Cancel();
		}

	}
}
