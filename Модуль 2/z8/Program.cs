using System;

// Абстрактный класс
abstract class Shape
{
    public string Name { get; set; }

    // Защищённый конструктор — вызывается только из наследников 
    protected Shape(string name)
    {
        Name = name;
    }

    // Абстрактный метод
    public abstract double Area();

    // Виртуальный метод
    public virtual void PrintInfo()
    {
        Console.WriteLine($"{Name}: площадь = {Area():F2}");
    }
}

// Круг
class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(double radius) : base("Круг")
    {
        Radius = radius;
    }

    // Обязателная реализация абстрактного метода
    public override double Area()
    {
        return Math.PI * Radius * Radius; 
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
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height) : base("Прямоугольник")
    {
        Width = width;
        Height = height;
    }

    public override double Area()
    {
        return Width * Height; 
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"Прямоугольник ({Width} × {Height})");
        base.PrintInfo();
    }
}

// Треугольник
class Triangle : Shape
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    public Triangle(double a, double b, double c) : base("Треугольник")
    {
        A = a;
        B = b;
        C = c;
    }

    public override double Area()
    {
        // Формула Герона
        double p = (A + B + C) / 2;
        return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"Треугольник ({A}, {B}, {C})");
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
            new Triangle(3, 4, 5)
        };

        foreach (Shape s in shapes)
        {
            s.PrintInfo(); // полиморфизм: вызовется версия наследника
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}