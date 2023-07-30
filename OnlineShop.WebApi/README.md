# Использование Entity Framework Core в проекте ASP.NET Core

 EF это object-relational mapper (ORM) или инструмент, связывающий объектную модель, с которой мы работаем в коде (C# классы, коллекции, свойства) с реляционной моделью базы данных (таблица, столбец, запись, связи etc). Основной объект, который предоставляет EF для работы с базой данных это класс производный от DbContext. Класс содержит в себе набор объектов-коллекций DbSet, которые чаще всего соотносятся с таблицами базы данных. Для доступа к этим данным, мы обращаемся к этим коллекциям с помощью LINQ запросов, которые за кадром транслируются в SQL при вызове методов ToArray, ToList, FirstOrDefault и т.д., и работаем с данными также, как и с обычными C# объектами.

```С#
public class AdventureWorksContext : DbContext
{
    public virtual DbSet<Product> Products { get; set; }
    ...
}
...
public void ApplicationLogic()
{
    using var context = new AdventureWorksContext();
    // get list of products with filter
    var bookProducts = context.Products.Where(p => p.Type == "Book").ToList();
    // get single product by name
    var singleBook = context.Products.Where(p => p.Name == "Harry Potter").FirstOrDefault();
    // do data handling
    ...
}
```

## Реализуйте полноценный CRUD функционал:
1. Эндпоинт для добавления новых товаров в БД (C);
2. Эндпоинт для чтения конкретного товара по его Id (R);
3. Эндпоинт для изменения товара (U);
   Из параметров запроса productId [FromQuery], из тела запроса product [FromBody].
4. Эндпоинт для удаления конкретного товара (D).
