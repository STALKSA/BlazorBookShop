# Blazor WebAssembly

Приложения Blazor WebAssembly (WASM) выполняются на стороне клиента в браузере в среде выполнения .NET на основе WebAssembly. Приложение Blazor, его зависимости и среда выполнения .NET загружаются в браузер. Приложение выполняется непосредственно в потоке пользовательского интерфейса браузера. Обновления пользовательского интерфейса и обработка событий происходят в рамках одного и того же процесса. Ресурсы приложения развертываются в виде статических файлов на веб-сервере или в службе, способной предоставлять статический контент клиентам.

![пример](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models/_static/blazor-webassembly.png?view=aspnetcore-7.0)

## Преимущества использования

* После загрузки приложения с сервера отсутствует зависимость на стороне сервера .NET, поэтому приложение остается работоспособным, даже если сервер отключается.
* Ресурсы и возможности клиента используются в полной мере.
* Работа перекладывается с сервера на клиента.
* Веб-сервер ASP.NET Core не требуется для размещения приложения. Возможны сценарии бессерверного развертывания, например обслуживание приложения из сети доставки контента (CDN).

# Задачи

- [x] Создать проект Blazor WebAssembly.(+https://github.com/MudBlazor/Templates)
- [x] Реализовать страницу с информацией о товаре.
- [x] При нажатии на название товара из любой страницы реализуйте переход к странице с информацией о товаре. (NavigationManager)
- [x] Выделите плитку товара (которую показываете в каталоге) в отдельный Razor-компонент и переиспользуйте его в каталоге и корзине.
- [x] Для страницы каталога разделите код и UI по разным файлам.
