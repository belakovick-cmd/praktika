using System;

class Program
{
    static void Main()
    {
        Random random = new Random();
        int[] array = new int[10];

        // Заполняем массив случайными числами из диапазона [-100, 100]
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.Next(-100, 101);
        }

        Console.WriteLine("Исходный массив:");
        Console.WriteLine(string.Join(", ", array));

        Console.Write("\nВведите целое число для замены: ");
        int number = int.Parse(Console.ReadLine());

        // Поиск индекса максимального элемента
        int maxIndex = 0;
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > array[maxIndex])
                maxIndex = i;
        }

        Console.WriteLine($"\nМаксимальный элемент: {array[maxIndex]} (индекс {maxIndex})");

        // Замена максимального элемента введённым числом
        array[maxIndex] = number;

        Console.WriteLine("\nИзменённый массив:");
        Console.WriteLine(string.Join(", ", array));
        Console.ReadKey();
    }
}