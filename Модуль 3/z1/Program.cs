using System;

// Делегат для метода без параметров
delegate double AreaCalculator();

// Базовый класс
abstract class Shape
{
    // Абстрактный метод 
    public abstract double Area();
}

// Наследники

// Круг
class Circle : Shape
{
    public double Radius;                          // радиус круга
    public Circle(double r) => Radius = r;         // конструктор
    public override double Area() => Math.PI * Radius * Radius;   
}

// Прямоугольник
class Rectangle : Shape
{
    public double W, H;                            // ширина и высота
    public Rectangle(double w, double h) { W = w; H = h; }        // конструктор
    public override double Area() => W * H;        // S = a * b
}

// Треугольник
class Triangle : Shape
{
    public double A, B, C;                         // стороны треугольника
    public Triangle(double a, double b, double c) { A = a; B = b; C = c; }   // конструктор
    public override double Area()
    {
        // Формула Герона
        double p = (A + B + C) / 2;                // полупериметр
        return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
    }
}

class Program
{
    static void Main()
    {
        // Создание фигур
        Shape circle = new Circle(5);
        Shape rectangle = new Rectangle(4, 6);
        Shape triangle = new Triangle(3, 4, 5);

        // Создание делегатов — по одному на каждую фигуру
        AreaCalculator calc1 = circle.Area;        // указывает на Circle.Area
        AreaCalculator calc2 = rectangle.Area;     // указывает на Rectangle.Area
        AreaCalculator calc3 = triangle.Area;      // указывает на Triangle.Area

        // Динамический вызов через делегата
        Console.WriteLine($"Круг: {calc1():F2}");
        Console.WriteLine($"Прямоугольник: {calc2():F2}");
        Console.WriteLine($"Треугольник: {calc3():F2}");

        Console.ReadKey();  
    }
}