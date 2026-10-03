using System;

class Shape
{
    public virtual double Area()
    {
        return 0; 
    }

    public virtual double Perimeter()
    {
        return 0; 
    }

    // Общий метод вывода 
    public virtual void PrintInfo()
    {
        Console.WriteLine($"Площадь: {Area():F2}, Периметр: {Perimeter():F2}");
    }
}

// Круг
class Circle : Shape
{
    // Автоматическое создание приватного поля
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius; // Сохранение радиуса 
    }

    // 
    public override double Area()
    {
        return Math.PI * Radius * Radius; 
    }

    public override double Perimeter()
    {
        return 2 * Math.PI * Radius; 
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"Круг (радиус = {Radius})");
        base.PrintInfo(); 
    }
}

// Прямоугольник
class Rectangle : Shape
{
    public double Width { get; set; }  // ширина
    public double Height { get; set; } // высота

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double Area()
    {
        return Width * Height; 
    }

    public override double Perimeter()
    {
        return 2 * (Width + Height); // P = 2(a+b)
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"Прямоугольник ({Width} × {Height})");
        base.PrintInfo();
    }
}

class Program
{
    static void Main()
    {
        Shape[] shapes = new Shape[]
        {
            new Circle(5),          
            new Rectangle(4, 6),    
            new Circle(2.5),
            new Rectangle(10, 3)
        };

        // Проход по всем фигурам и вызов PrintInfo
        foreach (Shape shape in shapes)
        {
            shape.PrintInfo();
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}