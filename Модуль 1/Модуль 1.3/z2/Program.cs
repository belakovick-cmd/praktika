using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите предельную сумму: ");
        int limit = int.Parse(Console.ReadLine());

        Random random = new Random();

        // Временный массив 
        int[] temp = new int[limit];
        int count = 0;
        int sum = 0;

        // Заполнение массива, пока сумма не превысит лимит
        while (true)
        {
            int next = random.Next(1, 10); // [1, 9]
            if (sum + next > limit)
                break;

            temp[count] = next;
            sum += next;
            count++;
        }

        // Создание массива нужного размера
        int[] result = new int[count];
        Array.Copy(temp, result, count);

        // Вывод
        Console.WriteLine($"\nМассив из {count} элементов:");
        Console.WriteLine(string.Join(" ", result));
        Console.WriteLine($"Сумма = {sum} (лимит = {limit})");

        Console.ReadKey();
    }
}