using System;

// Интерфейс "Книга"
// Обязывает все книги уметь проверять доступность и выдаваться читателю
interface IBook
{
    string Title { get; }        // название книги
    string Author { get; }       // автор
    bool IsAvailable();          // доступна ли книга
    bool Checkout();             // выдать книгу (true — успешно)
    void Return();               // вернуть книгу
}

// Базовый класс с общими полями
abstract class BookBase : IBook
{
    public string Title { get; protected set; }
    public string Author { get; protected set; }

    protected bool isCheckedOut;   // выдана ли книга

    protected BookBase(string title, string author)
    {
        Title = title;
        Author = author;
        isCheckedOut = false;      // по умолчанию книга в библиотеке
    }

    // Доступна = не выдана
    public virtual bool IsAvailable() => !isCheckedOut;

    // Выдать книгу
    public virtual bool Checkout()
    {
        if (isCheckedOut)
            return false;          // уже выдана - нельзя

        isCheckedOut = true;
        return true;               // успешно выдана
    }

    // Вернуть книгу
    public virtual void Return()
    {
        isCheckedOut = false;
    }
}

// Обычная книга
class RegularBook : BookBase
{
    public RegularBook(string title, string author)
        : base(title, author) { }
}

// Редкая книга - нельзя выдать на дом, только читальный зал
class RareBook : BookBase
{
    public RareBook(string title, string author)
        : base(title, author) { }

    // Переопределение - редкие книги не выдаются
    public override bool Checkout()
    {
        Console.WriteLine("  [Редкая книга] Выдача только в читальном зале.");
        return false;
    }

    public override bool IsAvailable()
    {
        // Редкая книга всегда "доступна" в читальном зале
        return true;
    }
}

// Электронная книга — всегда доступна, неограниченное число копий
class EBook : BookBase
{
    public EBook(string title, string author)
        : base(title, author) { }

    public override bool IsAvailable() => true;   // всегда доступна

    public override bool Checkout()
    {
        Console.WriteLine("  [Электронная книга] Ссылка для скачивания отправлена.");
        return true;                              // всегда успешно
    }

    public override void Return()
    {
        // Нечего возвращать - электронная копия остаётся у читателя
    }
}

class Program
{
    static void Main()
    {
        // Массив книг 
        IBook[] books =
        {
            new RegularBook("Война и мир", "Л.Н. Толстой"),
            new RareBook("Первое издание Пушкина", "А.С. Пушкин"),
            new EBook("C# для начинающих", "И. Иванов"),
            new RegularBook("Мастер и Маргарита", "М.А. Булгаков")
        };

        Console.WriteLine("Каталог библиотеки\n");

        // Вывод информации о всех книгах
        foreach (IBook b in books)
        {
            Console.WriteLine($"\"{b.Title}\" — {b.Author}");
            Console.WriteLine($"  Доступна: {(b.IsAvailable() ? "да" : "нет")}");
            Console.WriteLine();
        }

        // Выдача книг
        Console.WriteLine("Попытка выдачи\n");

        foreach (IBook b in books)
        {
            Console.WriteLine($"Выдаём \"{b.Title}\":");
            bool success = b.Checkout();
            Console.WriteLine($"  Результат: {(success ? "выдана" : "отказано")}");
            Console.WriteLine();
        }

        // Проверка доступности после выдачи
        Console.WriteLine("Доступность после выдачи\n");

        foreach (IBook b in books)
        {
            Console.WriteLine($"\"{b.Title}\": {(b.IsAvailable() ? "доступна" : "выдана")}");
        }

        // Возврат одной книги
        Console.WriteLine("\nВозврат \"Война и мир\"\n");
        books[0].Return();
        Console.WriteLine($"\"{books[0].Title}\": {(books[0].IsAvailable() ? "доступна" : "выдана")}");

        Console.ReadKey();
    }
}