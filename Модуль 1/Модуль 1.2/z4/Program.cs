using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество элементов K: ");
        int k = int.Parse(Console.ReadLine());

        Console.Write("Введите начало диапазона A: ");
        int a = int.Parse(Console.ReadLine());

        Console.Write("Введите конец диапазона B: ");
        int b = int.Parse(Console.ReadLine());

        Random random = new Random();
        int[] array = new int[k];

        // Заполнение массива случайными числами из [A, B)
        for (int i = 0; i < k; i++)
        {
            array[i] = random.Next(a, b);
        }

        // Вывод массива
        Console.WriteLine("\nМассив:");
        for (int i = 0; i < k; i++)
        {
            Console.Write($"{array[i],6}");
            if ((i + 1) % 10 == 0)
                Console.WriteLine();
        }
        if (k % 10 != 0) Console.WriteLine();

        // Поиск индексов min и max
        int minIndex = 0, maxIndex = 0;
        for (int i = 1; i < k; i++)
        {
            if (array[i] < array[minIndex]) minIndex = i;
            if (array[i] > array[maxIndex]) maxIndex = i;
        }

        Console.WriteLine($"\nМинимум: {array[minIndex]} (индекс {minIndex})");
        Console.WriteLine($"Максимум: {array[maxIndex]} (индекс {maxIndex})");

        // Определение границ диапазона (от меньшего индекса к большему)
        int start = Math.Min(minIndex, maxIndex);
        int end = Math.Max(minIndex, maxIndex);

        Console.WriteLine($"\nЭлементы между индексами {start} и {end} (включительно):");
        for (int i = start; i <= end; i++)
        {
            Console.Write($"{array[i],6}");
        }
        Console.WriteLine();

        Console.ReadKey();
    }
}