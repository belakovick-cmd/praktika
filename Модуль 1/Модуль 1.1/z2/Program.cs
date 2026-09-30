using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите радиус круга: ");
        double radius = double.Parse(Console.ReadLine());

        double area = Math.PI * radius * radius;

        Console.WriteLine($"Площадь круга с радиусом {radius} равна {area:F2}");
        Console.ReadKey();
    }
}