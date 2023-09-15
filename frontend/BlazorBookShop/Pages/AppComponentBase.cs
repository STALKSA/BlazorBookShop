using Blazored.LocalStorage;
using BookShop.HttpApiClient;
using Microsoft.AspNetCore.Components;
#pragma warning disable CS8618


namespace BlazorBookShop.Pages
{
    public class AppComponentBase : ComponentBase
    {
        [Inject] protected BookShopClient ShopClient { get; private set; }
        [Inject] protected ILocalStorageService LocalStorage { get; private set; }
        [Inject] protected AppState State { get; private set; }

        //protected bool IsTokenChecked { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (State.IsTokenChecked) return;
            //State.IsTokenChecked = true;


            var token = await LocalStorage.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                ShopClient.SetAuthorizationToken(token);
                //State.IsTokenChecked = false;
            }
        }
      

    }
}
