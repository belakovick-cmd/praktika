using System;

class Program
{
    static void Main()
    {
        Random random = new Random();
        double[] array = new double[10];

        // Заполнение массива случайными числами из [-10, 10)
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.NextDouble() * 20 - 10;
        }

        // Вывод исходного массива
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < array.Length; i++)
        {
            Console.WriteLine($"[{i}] = {array[i]:F4}");
        }

        // Создание массива индексов: 0, 1, 2, ..., 9
        int[] indices = new int[array.Length];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = i;
        }

        // Сортировка индексов по значениям элементов массива
        Array.Sort(indices, (x, y) => array[x].CompareTo(array[y]));

        // Вывод массива индексов
        Console.WriteLine("\nМассив индексов в порядке возрастания значений:");
        Console.WriteLine(string.Join(" ", indices));

        // Вывод элементов в порядке возрастания
        Console.WriteLine("\nЭлементы в порядке возрастания:");
        foreach (int idx in indices)
        {
            Console.WriteLine($"[{idx}] = {array[idx]:F4}");
        }

        Console.ReadKey();
    }
}
