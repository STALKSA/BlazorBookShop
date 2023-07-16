using BlazorBookShop.Interfaces;
using BlazorBookShop.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorBookShop.Pages
{
	public partial class CatalogPage : IDisposable
	{
		[Inject]
		private IBookShopClient ShopClient { get; set; } = null!;

		private Product[]? _products;
		private CancellationTokenSource _cts = new();

		protected override async Task OnInitializedAsync()
		{
			_products = await ShopClient.GetProducts(_cts.Token);
			//StateHasChanged();
		}
		void NavigateToProductPage(Guid id)
	    {
            NavigationManager.NavigateTo($"/products/{id.ToString()}");
	    }

		public void Dispose()
		{
			_cts.Cancel();
		}

	}
}
