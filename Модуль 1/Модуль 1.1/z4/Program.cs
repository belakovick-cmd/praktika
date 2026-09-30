using System;

class Program
{
    static void Main()
    {
        Random random = new Random();
        int[] numbers = new int[20];

        // Заполнение массива случайными числами
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = random.Next(1, 101); // числа от 1 до 100
        }

        // Вывод массива
        Console.WriteLine("Массив: " + string.Join(", ", numbers));

        // Находим максимум и минимум
        int max = numbers[0];
        int min = numbers[0];

        for (int i = 1; i < numbers.Length; i++)
        {
            if (numbers[i] > max) max = numbers[i];
            if (numbers[i] < min) min = numbers[i];
        }

        Console.WriteLine($"Максимальное значение: {max}");
        Console.WriteLine($"Минимальное значение: {min}");
        Console.ReadKey();
    }
}