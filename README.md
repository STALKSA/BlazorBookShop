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

## Репозиторий

1. Добавьте редактор товаров, чтобы он позволял хотя бы добавлять и удалять товары из каталога.
- [ ] Добавьте функционал редактирования товаров.
2. Доделайте все практические задания и предыдущие ДЗ

## Generic-репозиторий, контроллеры

1. Добавьте класс Account и создайте для него репозиторий
2. Упростите все репозитории в программе, путем наследования от generic repo

