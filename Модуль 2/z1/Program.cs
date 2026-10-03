using System;

class Person
{
    // Приватные поля
    private string name;
    private int age;
    private string address;

    // Конструктор
    public Person(string name, int age, string address)
    {
        this.name = name;
        this.age = age;
        this.address = address;
    }

    // Методы установки значений
    public void SetName(string name) => this.name = name;
    public void SetAge(int age) => this.age = age;
    public void SetAddress(string address) => this.address = address;

    // Методы получения значений 
    public string GetName() => name;
    public int GetAge() => age;
    public string GetAddress() => address;

    // Метод для вывода информации
    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {name}");
        Console.WriteLine($"Возраст: {age}");
        Console.WriteLine($"Адрес: {address}");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        // Создание объектов
        Person p1 = new Person("Иван Петров", 25, "г. Минск, ул. Ленина, 10");
        Person p2 = new Person("Мария Сидорова", 30, "г. Орша, ул. Мира, 5");

        // Вывод информации
        Console.WriteLine("Информация о людях:\n");
        p1.PrintInfo();
        p2.PrintInfo();
        
        Console.ReadKey();
    }
}