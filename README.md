# BlazorWebAssemblyApp

## Реализация API-клиента

1. Доделайте API-клиент.
2. Внедрите зависимость клиента в проект Blazor WASM.
3. Выведите список товаров на странице фронтэнда(Подсказка: перезапишите метод OnInitializedAsync.Добавьте CORS)
```
   builder.Services.AddCors();
… var app = builder.Build(); …

app.UseCors(policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
});
```
- [ ] Реализуйте API-клиент при помощи библиотеки Refit
