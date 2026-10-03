using System;

interface IDrawable
{
    void Draw(); 
}

class Circle : IDrawable // Обязуется реализовать все метода класса
{
    public double Radius { get; set; }

    public Circle(double radius) => Radius = radius; // короткий конструктор

    public void Draw()
    {
        Console.WriteLine($"Рисуется круг радиусом {Radius}");
    }
}

// Класс прямоугольник
class Rectangle : IDrawable
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public void Draw()
    {
        Console.WriteLine($"Рисуется прямоугольник {Width} x {Height}");
    }
}

// Класс треугольник
class Triangle : IDrawable
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

    public void Draw()
    {
        Console.WriteLine($"Рисуется треугольник со сторонами {A}, {B}, {C}");
    }
}

class Program
{
    static void Main()
    {
        // Массив объектов, реализующих интерфейс
        IDrawable[] drawables = new IDrawable[]
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(3, 4, 5),
            new Circle(2.5)
        };

        // Вызов метода Draw для каждого
        foreach (IDrawable d in drawables)
        {
            d.Draw();
        }

        Console.ReadKey();
    }
}