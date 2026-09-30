using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива N: ");
        int n = int.Parse(Console.ReadLine());

        Random random = new Random();
        double[] array = new double[n];

        for (int i = 0; i < n; i++)
        {
            array[i] = random.NextDouble() * 200 - 100;
        }

        // Вывод исходного массива
        Console.WriteLine("\nИсходный массив:");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"[{i}] = {array[i]:F4}");
        }

        // Поиск максимального по модулю элемента
        double maxAbs = Math.Abs(array[0]);
        for (int i = 1; i < n; i++)
        {
            if (Math.Abs(array[i]) > maxAbs)
                maxAbs = Math.Abs(array[i]);
        }

        Console.WriteLine($"\nМаксимальный по модулю элемент: {maxAbs:F4}");

        // Нормирование
        for (int i = 0; i < n; i++)
        {
            array[i] /= maxAbs;
        }

        // Вывод нормированного массива
        Console.WriteLine("\nНормированный массив:");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"[{i}] = {array[i]:F4}");
        }
        Console.ReadKey();
    }
}