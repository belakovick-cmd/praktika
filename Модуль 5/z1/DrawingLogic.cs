using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace z1
{
    // Типы фигур
    public enum ShapeType { Line, Circle, Rectangle }

    // Класс фигуры
    public class Shape
    {
        public ShapeType Type;
        public Point Start;
        public Point End;

        public Shape(ShapeType type, Point start, Point end)
        {
            Type = type;
            Start = start;
            End = end;
        }
    }

    // Класс логики рисования
    public static class DrawingLogic
    {
        public static List<Shape> Shapes = new List<Shape>();
        public static ShapeType CurrentShape = ShapeType.Line;
        public static Point StartPoint;
        public static Point EndPoint;
        public static bool IsDrawing = false;

        public static void StartDrawing(Point location)
        {
            IsDrawing = true;
            StartPoint = location;
            EndPoint = location;
        }

        public static void UpdateDrawing(Point location)
        {
            if (IsDrawing)
                EndPoint = location;
        }

        public static void FinishDrawing(Point location)
        {
            if (!IsDrawing) return;

            IsDrawing = false;
            EndPoint = location;
            Shapes.Add(new Shape(CurrentShape, StartPoint, EndPoint));
        }

        public static void ClearAll()
        {
            Shapes.Clear();
        }

        public static void DrawAll(Graphics g)
        {
            foreach (Shape shape in Shapes)
                DrawShape(g, shape.Type, shape.Start, shape.End);

            if (IsDrawing)
                DrawShape(g, CurrentShape, StartPoint, EndPoint);
        }

        private static void DrawShape(Graphics g, ShapeType type, Point start, Point end)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                switch (type)
                {
                    case ShapeType.Line:
                        g.DrawLine(pen, start, end);
                        break;

                    case ShapeType.Circle:
                        int dx = end.X - start.X;
                        int dy = end.Y - start.Y;
                        int radius = (int)Math.Sqrt(dx * dx + dy * dy);
                        g.DrawEllipse(pen, start.X - radius, start.Y - radius,
                                           radius * 2, radius * 2);
                        break;

                    case ShapeType.Rectangle:
                        int x = Math.Min(start.X, end.X);
                        int y = Math.Min(start.Y, end.Y);
                        int w = Math.Abs(end.X - start.X);
                        int h = Math.Abs(end.Y - start.Y);
                        g.DrawRectangle(pen, x, y, w, h);
                        break;
                }
            }
        }
    }
}