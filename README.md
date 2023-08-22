# BlazorWebAssemblyApp

## Реализация API-клиента

1. API-клиент.
2. Внедрить зависимость клиента в проект Blazor WASM.
3. Выведить список товаров на странице фронтэнда(Подсказка: перезаписать метод OnInitializedAsync. Добавить CORS)
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


## Репозиторий

1. Добавть редактор товаров, чтобы он позволял хотя бы добавлять и удалять товары из каталога.
- [ ] Добавить функционал редактирования товаров.

## Generic-репозиторий, контроллеры

1. Добавить класс Account и создать для него репозиторий
2. Упростить все репозитории в программе путем наследования от generic repo

## APIcontroller, результаты действий

1. Добавить отдельную модель данных для передачи в запросе на регистрацию.
2. Реализуйть валидацию модели регистрации при помощи библиотеки FluentValidator.

## Сервисы, виды моделей данных

1. Перенести бизнес логику регистрации в сервис AccountService.
2. Создать собственный эксепшн на случай, если аккаунт с таким же Email'ом уже зарегистрирован в системе.
3. На уровне контроллера обработать эксепшн о том, что пользователь с таким Email'ом уже зарегистрирован и верните соответствующий ответ.

## Обработка ошибок в APIclient

1. Вынести работу с данными в отдельный проект (репозиторий, DbContext).

## Хранение паролей

1. Реализовать регистрацию с корректным хранением пароля в БД.
2. Добавить action для авторизации. Если авторизация прошла успешно, то возвращать класс аккаунта.
3. Добавить страницу логина в Blazor WASM.
- [ ]  Реализовать метод расширения PostAsJsonAndDeserializeAsync для класса HttpClient.

## Middleware

1. Добавить логирование тела запросов и логирование тела ответов (используя подход из .net 6).
2. Реализовать подсчет переходов для всех страниц (HttpContext.Request.Path) (через класс Middleware).
- [ ] Добавить страницу /metrics с выводом данных о переходах в формате адрес страницы: кол-во переходов.
- [ ] (Клиент) Реализовать перехват 400-й и остальных ошибок и прокидывайте исключение с подробностями об ошибке из ShopClient.

