using System;

class Car
{
    public string Brand { get; set; }   // марка
    public string Model { get; set; }   // модель
    public int Year { get; set; }       // год выпуска

    public decimal Price { get; set; }  // базовая цена

    public Car(string brand, string model, int year, decimal price)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Price = price;
    }

    // Стоимость со скидкой
    public decimal GetPriceWithDiscount(decimal discountPercent)
    {
        // Новая цена = старая × (1 − скидка/100)
        return Price * (1 - discountPercent / 100);
    }

    // Стоимость НДС
    public decimal GetPriceWithVAT(decimal vatPercent = 20)
    {
        // Цена с НДС = цена × (1 + НДС/100)
        return Price * (1 + vatPercent / 100);
    }

    // Итоговая цена
    public decimal GetFinalPrice(decimal discountPercent, decimal vatPercent = 20)
    {
        decimal discounted = GetPriceWithDiscount(discountPercent); // сначала скидка
        return discounted * (1 + vatPercent / 100);                // потом НДС
    }

    public void PrintInfo()
    {
        Console.WriteLine($"{Brand} {Model}, {Year} г.");
        Console.WriteLine($"  Базовая цена: {Price:N2} руб.");
        Console.WriteLine($"  Со скидкой 10%: {GetPriceWithDiscount(10):N2} руб.");
        Console.WriteLine($"  С НДС 20%: {GetPriceWithVAT():N2} руб.");
        Console.WriteLine($"  Со скидкой 10% и НДС: {GetFinalPrice(10):N2} руб.");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        // Создание массива машин
        Car[] cars = new Car[]
        {
            new Car("Toyota", "Camry", 2022, 45000),
            new Car("BMW", "X5", 2023, 85000),
            new Car("Skoda", "Superb", 2017, 50000)
        };

        // Вывод информации о каждой
        foreach (Car car in cars)
        {
            car.PrintInfo();
        }

        Console.ReadKey();
    }
}