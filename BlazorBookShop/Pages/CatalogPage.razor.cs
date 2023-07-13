using BlazorBookShop.Models;



namespace BlazorBookShop.Pages
{
	public partial class CatalogPage
    {
        private Product[] products;

		protected override async Task OnInitializedAsync()
		{
			products = await BookShopClient.GetProducts();
			StateHasChanged();
		}
		void NavigateToProductPage(Guid id)
	    {
            NavigationManager.NavigateTo($"/products/{id.ToString()}");
	    }

    }
}
