using System;

// Интерфейс "Рисунок"
// Обязывает уметь рисовать линии, круги и прямоугольники
interface IDrawing
{
    void DrawLine(int x1, int y1, int x2, int y2);   // линия
    void DrawCircle(int x, int y, int radius);       // круг
    void DrawRectangle(int x, int y, int w, int h);  // прямоугольник
}

// Класс "Холст" 
class Canvas : IDrawing
{
    public string Name { get; }   // название холста

    public Canvas(string name)
    {
        Name = name;
    }

    // Рисование линии
    public void DrawLine(int x1, int y1, int x2, int y2)
    {
        Console.WriteLine($"  [Линия] из ({x1}, {y1}) в ({x2}, {y2})");
    }

    // Рисование круга
    public void DrawCircle(int x, int y, int radius)
    {
        Console.WriteLine($"  [Круг] центр ({x}, {y}), радиус {radius}");
    }

    // Рисование прямоугольника
    public void DrawRectangle(int x, int y, int w, int h)
    {
        Console.WriteLine($"  [Прямоугольник] угол ({x}, {y}), {w}х{h}");
    }
}

class Program
{
    static void Main()
    {
        // Создание холста
        IDrawing canvas = new Canvas("Мой холст");

        Console.WriteLine("Рисование на холсте\n");

        // Рисование через интерфейс
        canvas.DrawLine(0, 0, 100, 100);
        canvas.DrawCircle(50, 50, 30);
        canvas.DrawRectangle(10, 10, 80, 40);

        // Рисование ещё нескольких фигур
        canvas.DrawLine(20, 80, 120, 20);
        canvas.DrawCircle(100, 100, 15);

        Console.ReadKey();
    }
}