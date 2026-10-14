using System;

// Интерфейс "Фигура"
// Обязывает все фигуры уметь считать площадь и периметр
interface IShape
{
    double Area();        // площадь
    double Perimeter();   // периметр
}

// Круг
class Circle : IShape
{
    public double Radius { get; set; }   // радиус

    public Circle(double radius)
    {
        Radius = radius;
    }

    // Площадь круга
    public double Area()
    {
        return Math.PI * Radius * Radius;
    }

    // Периметр 
    public double Perimeter()
    {
        return 2 * Math.PI * Radius;
    }
}

// Прямоугольник
class Rectangle : IShape
{
    public double Width { get; set; }    // ширина
    public double Height { get; set; }   // высота

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    // Площадь: S = a·b
    public double Area()
    {
        return Width * Height;
    }

    // Периметр
    public double Perimeter()
    {
        return 2 * (Width + Height);
    }
}

// Треугольник
class Triangle : IShape
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    public Triangle(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    // Площадь по формуле Герона
    public double Area()
    {
        double p = (A + B + C) / 2;   // полупериметр
        return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
    }

    // Периметр
    public double Perimeter()
    {
        return A + B + C;
    }
}

class Program
{
    static void Main()
    {
        // Массив фигур — все реализуют IShape
        IShape[] shapes =
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(3, 4, 5)
        };

        // Проход по всем фигурам и вывод площади и периметра
        foreach (IShape shape in shapes)
        {
            Console.WriteLine(shape.GetType().Name);   // имя класса
            Console.WriteLine($"  Площадь:  {shape.Area():F2}");
            Console.WriteLine($"  Периметр: {shape.Perimeter():F2}");
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}