using System;

class Author
{
    public string Name { get; set; }      
    public int BirthYear { get; set; }    

    public Author(string name, int birthYear)
    {
        Name = name;
        BirthYear = birthYear;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Автор: {Name} (родился в {BirthYear})");
    }
}

class Book
{
    public string Title { get; set; }  // название книги
    public int Year { get; set; }      // год выпуска

    public Author Author { get; set; }

    public Book(string title, int year, Author author)
    {
        Title = title;
        Year = year;
        Author = author; // сохранение ссылки на объект автора
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Книга: \"{Title}\" ({Year} г.)");
        // Обращение к полям автора через точку
        Console.WriteLine($"  Автор: {Author.Name} (родился в {Author.BirthYear})");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        // Создание авторов
        Author pushkin = new Author("А.С. Пушкин", 1799);
        Author tolstoy = new Author("Л.Н. Толстой", 1828);
        Author bulgakov = new Author("М.А. Булгаков", 1891);

        // Создание книг

        Book[] books = new Book[]
        {
            new Book("Евгений Онегин", 1833, pushkin),
            new Book("Война и мир", 1869, tolstoy),
            new Book("Мастер и Маргарита", 1967, bulgakov),
            new Book("Анна Каренина", 1878, tolstoy) 
        };

        Console.WriteLine("Список книг:\n");
        foreach (Book book in books)
        {
            book.PrintInfo();
        }

        Console.ReadKey();
    }
}