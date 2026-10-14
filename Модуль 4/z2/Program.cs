using System;

// Интерфейс "Товар"
// Обязывает все товары уметь считать стоимость партии и показывать остаток
interface IProduct
{
    string Name { get; }              // название товара
    double Price { get; }             // цена за единицу
    int Quantity { get; }             // остаток на складе
    double GetTotalCost();            // стоимость всей партии
    int GetStock();                   // остаток на складе
}

// Базовый класс с общими полями
abstract class ProductBase : IProduct
{
    public string Name { get; protected set; }
    public double Price { get; protected set; }
    public int Quantity { get; protected set; }

    protected ProductBase(string name, double price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    // Стоимость партии = цена * количество
    public virtual double GetTotalCost() => Price * Quantity;

    // Остаток на складе
    public virtual int GetStock() => Quantity;
}

// Хлеб - продаётся поштучно
class Bread : ProductBase
{
    public Bread(double price, int quantity)
        : base("Хлеб", price, quantity) { }
}

// Молоко — за литр
class Milk : ProductBase
{
    public Milk(double price, int quantity)
        : base("Молоко", price, quantity) { }
}

// Сыр - за килограмм, со скидкой при большой партии
class Cheese : ProductBase
{
    public Cheese(double price, int quantity)
        : base("Сыр", price, quantity) { }

    // Скидка 10% при количестве > 5
    public override double GetTotalCost()
    {
        double total = Price * Quantity;
        if (Quantity > 5)
            total *= 0.9;   // −10%
        return total;
    }
}

// Яйца - продаются десятками
class Eggs : ProductBase
{
    public int Packs { get; }   // количество десятков

    public Eggs(double pricePerPack, int packs)
        : base("Яйца", pricePerPack, packs * 10)
    {
        Packs = packs;
    }

    // Стоимость = цена за десяток * количество десятков
    public override double GetTotalCost() => Price * Packs;

    public override int GetStock() => Quantity;   // всего яиц
}

class Program
{
    static void Main()
    {
        // Массив товаров
        IProduct[] products =
        {
            new Bread(1.20, 30),
            new Milk(2.50, 15),
            new Cheese(12.00, 8),
            new Eggs(3.50, 5)
        };

        Console.WriteLine("Товары в магазине\n");

        double grandTotal = 0;

        foreach (IProduct p in products)
        {
            double cost = p.GetTotalCost();

            Console.WriteLine($"Товар: {p.Name}");
            Console.WriteLine($"  Цена: {p.Price:F2} руб.");
            Console.WriteLine($"  Остаток: {p.GetStock()} шт.");
            Console.WriteLine($"  Стоимость партии: {cost:F2} руб.");
            Console.WriteLine();

            grandTotal += cost;
        }

        Console.WriteLine($"Общая стоимость всех товаров: {grandTotal:F2} руб.");

        Console.ReadKey();
    }
}