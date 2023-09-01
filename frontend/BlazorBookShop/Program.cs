using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorBookShop;
using MudBlazor.Services;
using BookShop.HttpApiClient;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton<IBookShopClient>(new BookShopClient(host: "https://localhost:7061/"));

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
